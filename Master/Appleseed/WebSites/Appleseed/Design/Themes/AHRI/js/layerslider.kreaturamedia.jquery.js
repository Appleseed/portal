(function($) {

	$.fn.layerSlider = function( options ){

		// Initializing

		if( (typeof(options)).match('object|undefined') ){
			return this.each(function(i){
				new layerSlider(this, options);
			});
		}else{
			if( options == 'data' ){
				var lsData = $(this).data('LayerSlider');
				if( lsData ){
					return lsData;
				}
			}else{
				return this.each(function(i){

					// Control functions: prev, next, start, stop & change

					var lsData = $(this).data('LayerSlider');
					if( lsData ){
						if( !lsData.g.isAnimating ){
							if( typeof(options) == 'number' ){
								if( options > 0 && options < lsData.g.layersNum + 1 && options != lsData.g.curLayerIndex ){
									lsData.change(options);
								}						
							}else{
								switch(options){
									case 'prev':
										lsData.o.cbPrev();
										lsData.prev('clicked');
										break;
									case 'next':
										lsData.o.cbNext();
										lsData.next('clicked');
										break;
									case 'start':
										if( !lsData.g.autoSlideshow ){
											lsData.o.cbStart();
											lsData.g.originalAutoSlideshow = true;
											lsData.start();
										}							
										break;
								}
							}
						}
						if( ( lsData.g.autoSlideshow || ( !lsData.g.autoSlideshow && lsData.g.originalAutoSlideshow ) ) && options == 'stop' ){
							lsData.o.cbStop();
							lsData.g.originalAutoSlideshow = false;
							lsData.g.curLayer.find('iframe[src*="www.youtu"], iframe[src*="player.vimeo"]').each(function(){
								
								// Clearing videoTimeouts

								clearTimeout( $(this).data( 'videoTimer') );
							});
							
							lsData.stop();
						}
					}
				});				
			}
		}
	};

	// LayerSlider methods

	var layerSlider = function(el, options) {

		var ls = this;
		ls.$el = $(el).addClass('ls-container');
		ls.$el.data('LayerSlider', ls);

		ls.init = function(){

			// Setting options (user settings) and global (not modificable) parameters
			
			ls.o = $.extend({},layerSlider.options, options);
			ls.g = $.extend({},layerSlider.global);

			// REPLACED FEATURE: BUGFIX v1.6 if there is only ONE layer, duplicating it

/*			
			if( $(el).find('.ls-layer').length == 1 ){
				$(el).find('.ls-layer:eq(0)').clone().appendTo( $(el) );
			}
*/
			// REPLACED FEATURE v2.0 If there is only ONE layer, instead of duplicating it, turning off slideshow and loops, hiding all controls, etc.
			
			if( $(el).find('.ls-layer').length == 1 ){
				ls.o.autoStart = false;
				ls.o.navPrevNext = false;
				ls.o.navStartStop = false;
				ls.o.navButtons	 = false;
				ls.o.loops = 0;
				ls.o.forceLoopNum = false;
				ls.o.autoPauseSlideshow	= true;
				ls.o.firstLayer = 1;
			}			

			// Original width

			ls.g.sliderOriginalWidth = $(el)[0].style.width;

			// Storing unique settings of layers and sublayers into object.data

			$(el).find('.ls-layer, *[class*="ls-s"]').each(function(){

				if( $(this).attr('rel') || $(this).attr('style') ){
					if( $(this).attr('rel') ){
						var params = $(this).attr('rel').toLowerCase().split(';');
					}else{
						var params = $(this).attr('style').toLowerCase().split(';');						
					}
					for(x=0;x<params.length;x++){
						param = params[x].split(':');
						if( param[0].indexOf('easing') != -1 ){
							param[1] = ls.ieEasing( param[1] );
						}
						$(this).data( $.trim(param[0]), $.trim(param[1]) );
					}
				}

				// NEW FEATURE v1.7 making the slider responsive - we have to use style.left instead of jQuery's .css('left') function!

				$(this).data( 'originalLeft', $(this)[0].style.left );
				$(this).data( 'originalTop', $(this)[0].style.top );
			});

			// NEW FEATURE v2.0 linkTo

			$(el).find('*[class*="ls-linkto-"]').each(function(){
				var lClasses = $(this).attr('class').split(' ');
				for( var ll=0; ll<lClasses.length; ll++ ){
					if( lClasses[ll].indexOf('ls-linkto-') != -1 ){
						var linkTo = parseInt( lClasses[ll].split('ls-linkto-')[1] );
						$(this).click(function(e){
							e.preventDefault();
							$(el).layerSlider( linkTo );
						});
					}
				}
			});

			// Setting variables

			ls.g.layersNum = $(el).find('.ls-layer').length;
			ls.o.firstLayer = ls.o.firstLayer < ls.g.layersNum + 1 ? ls.o.firstLayer : 1;
			ls.o.firstLayer = ls.o.firstLayer < 1 ? 1 : ls.o.firstLayer;
			
			// NEW FEATURE v2.0 loops
			
			ls.g.nextLoop = 1;
			
			if( ls.o.animateFirstLayer ){
				ls.g.nextLoop = 0;
			}
			
			// NEW FEATURE v2.0 videoPreview

			// Youtube videos
			
			$(el).find('iframe[src*="www.youtu"]').each(function(){
				if( $(this).parent('[class*="ls-s"]') ){

					var iframe = $(this);

					// Getting thumbnail
					
					$.getJSON('http://gdata.youtube.com/feeds/api/videos/' + $(this).attr('src').split('embed/')[1].split('?')[0] + '?v=2&alt=json&callback=?', function(data) {

						iframe.data( 'videoDuration', parseInt(data['entry']['media$group']['yt$duration']['seconds']) * 1000 );
					});
					
					var vpContainer = $('<div>').addClass('ls-vpcontainer').appendTo( $(this).parent() );

					$('<img>').appendTo( vpContainer ).addClass('ls-videopreview').attr('src', 'http://img.youtube.com/vi/' + $(this).attr('src').split('embed/')[1].split('?')[0] + '/' + ls.o.youtubePreview );
					$('<div>').appendTo( vpContainer ).addClass('ls-playvideo');

					$(this).parent().css({
						width : $(this).width(),
						height : $(this).height()
					}).click(function(){

						ls.g.isAnimating = true;

						if( ls.g.paused ){
							if( ls.o.autoPauseSlideshow != false ){
								ls.g.paused = false;								
							}
							ls.g.originalAutoSlideshow = true;
						}else{
							ls.g.originalAutoSlideshow = ls.g.autoSlideshow;
						}

						if( ls.o.autoPauseSlideshow != false ){
							ls.stop();
						}

						ls.g.pausedByVideo = true;

						$(this).find('iframe').attr('src', $(this).find('iframe').data('videoSrc') );
						$(this).find('.ls-vpcontainer').delay(ls.g.v.d).fadeOut(ls.g.v.fo, function(){							
							if( ls.o.autoPauseSlideshow == 'auto' && ls.g.originalAutoSlideshow == true ){
								var videoTimer = setTimeout(function() {
										ls.start();
								}, iframe.data( 'videoDuration') - ls.g.v.d );
								iframe.data( 'videoTimer', videoTimer );
							}
							ls.g.isAnimating = false;
						});
					});

					var sep = '&';
					
					if( $(this).attr('src').indexOf('?') == -1 ){
						sep = '?';
					}

					$(this).data( 'videoSrc', $(this).attr('src') + sep + 'autoplay=1' );
					$(this).attr('src','');
				}
			});

			// Vimeo videos

			$(el).find('iframe[src*="player.vimeo"]').each(function(){
				if( $(this).parent('[class*="ls-s"]') ){

					var iframe = $(this);

					// Getting thumbnail

					var vpContainer = $('<div>').addClass('ls-vpcontainer').appendTo( $(this).parent() );

					$.getJSON('http://vimeo.com/api/v2/video/'+ ( $(this).attr('src').split('video/')[1].split('?')[0] ) +'.json?callback=?', function(data){

						$('<img>').appendTo( vpContainer ).addClass('ls-videopreview').attr('src', data[0]['thumbnail_large'] );						
						iframe.data( 'videoDuration', parseInt( data[0]['duration'] ) * 1000 );
						$('<div>').appendTo( vpContainer ).addClass('ls-playvideo');						
					});


					$(this).parent().css({
						width : $(this).width(),
						height : $(this).height()
					}).click(function(){

						ls.g.isAnimating = true;

						if( ls.g.paused ){
							if( ls.o.autoPauseSlideshow != false ){
								ls.g.paused = false;								
							}
							ls.g.originalAutoSlideshow = true;
						}else{
							ls.g.originalAutoSlideshow = ls.g.autoSlideshow;
						}

						if( ls.o.autoPauseSlideshow != false ){
							ls.stop();
						}

						ls.g.pausedByVideo = true;

						$(this).find('iframe').attr('src', $(this).find('iframe').data('videoSrc') );
						$(this).find('.ls-vpcontainer').delay(ls.g.v.d).fadeOut(ls.g.v.fo, function(){
							if( ls.o.autoPauseSlideshow == 'auto' && ls.g.originalAutoSlideshow == true ){
								var videoTimer = setTimeout(function() {
										ls.start();
								}, iframe.data( 'videoDuration') - ls.g.v.d );
								iframe.data( 'videoTimer', videoTimer );
							}
							ls.g.isAnimating = false;
						});
					});

					var sep = '&';
					
					if( $(this).attr('src').indexOf('?') == -1 ){
						sep = '?';
					}

					$(this).data( 'videoSrc', $(this).attr('src') + sep + 'autoplay=1' );
					$(this).attr('src','');
				}
			});

			// NEW FEATURE v1.7 animating first layer
			
			if( ls.o.animateFirstLayer ){
				ls.o.firstLayer = ls.o.firstLayer - 1 == 0 ? ls.g.layersNum : ls.o.firstLayer-1;
			}

			ls.g.curLayerIndex = ls.o.firstLayer;
			ls.g.curLayer = $(el).find('.ls-layer:eq('+(ls.g.curLayerIndex-1)+')');				
			
			// NEW FEATURE v1.7 making the slider responsive

			ls.g.sliderWidth = function(){
				return $(el).width();
			}
			
			ls.g.sliderHeight = function(){
				return $(el).height();
			}

			// NEW FEATURE v1.7 added yourLogo
			
			if( ls.o.yourLogo ){
				var yourLogo = $('<img>').appendTo($(el)).attr( 'src', ls.o.yourLogo ).attr('style', ls.o.yourLogoStyle );

				// NEW FEATURES v1.8 added yourLogoLink & yourLogoTarget

				if( ls.o.yourLogoLink != false ){
					$('<a>').appendTo($(el)).attr( 'href', ls.o.yourLogoLink ).attr('target', ls.o.yourLogoTarget ).css({
						textDecoration : 'none',
						outline : 'none'
					}).append( yourLogo );
				}
			}

			// Moving all layers to .ls-inner container

			$(el).find('.ls-layer').wrapAll('<div class="ls-inner"></div>');

			// Adding styles

			if( $(el).css('position') == 'static' ){
				$(el).css('position','relative');
			}

			$(el).find('.ls-inner').css({
				backgroundColor : ls.o.globalBGColor
			});
			
			if( ls.o.globalBGImage ){
				$(el).find('.ls-inner').css({
					backgroundImage : 'url('+ls.o.globalBGImage+')'
				});
			}

			$(el).find('.ls-bg').css({
				marginLeft : - ( ls.g.sliderWidth() / 2 )+'px',
				marginTop : - ( ls.g.sliderHeight() / 2 )+'px'
			});

			// Creating navigation

			if( ls.o.navPrevNext ){

				$('<a class="ls-nav-prev" href="#" />').click(function(e){
					e.preventDefault();
					$(el).layerSlider('prev');
				}).appendTo($(el));

				$('<a class="ls-nav-next" href="#" />').click(function(e){
					e.preventDefault();
					$(el).layerSlider('next');
				}).appendTo($(el));					
			}

			if( ls.o.navStartStop || ls.o.navButtons ){
				
				$('<div class="ls-bottom-nav-wrapper" />').appendTo( $(el) );

				if( ls.o.navButtons ){
					
					$('<span class="ls-bottom-slidebuttons" />').appendTo( $(el).find('.ls-bottom-nav-wrapper') );

					for(x=1;x<ls.g.layersNum+1;x++){

						$('<a href="#"></a>').appendTo( $(el).find('.ls-bottom-slidebuttons') ).click(function(e){
							e.preventDefault();
							$(el).layerSlider( ($(this).index() + 1) );
						});
					}
					$(el).find('.ls-bottom-slidebuttons a:eq('+(ls.o.firstLayer-1)+')').addClass('ls-nav-active');
				}

				if( ls.o.navStartStop ){
					
					$('<a class="ls-nav-start" href="#" />').click(function(e){
						e.preventDefault();
						$(el).layerSlider('start');
					}).prependTo( $(el).find('.ls-bottom-nav-wrapper') );

					$('<a class="ls-nav-stop" href="#" />').click(function(e){
						e.preventDefault();
						$(el).layerSlider('stop');
					}).appendTo( $(el).find('.ls-bottom-nav-wrapper') );
				}else{

					$('<span class="ls-nav-sides ls-nav-sideleft" />').prependTo( $(el).find('.ls-bottom-nav-wrapper') );
					$('<span class="ls-nav-sides ls-nav-sideright" />').appendTo( $(el).find('.ls-bottom-nav-wrapper') );						
				}
			}

			// Adding keyboard navigation if turned on and if number of layers > 1

			if( ls.o.keybNav && $(el).find('.ls-layer').length > 1 ){
				
				$('body').bind('keydown',function(e){ 
					if( !ls.g.isAnimating ){
						if( e.which == 37 ){
							ls.o.cbPrev();							
							ls.prev('clicked');
						}else if( e.which == 39 ){
							ls.o.cbNext();							
							ls.next('clicked');
						}
					}
				});
			}

			// Adding touch-control navigation if number of layers > 1
			
			if('ontouchstart' in window && $(el).find('.ls-layer').length > 1 && ls.o.touchNav ){

			   $(el).bind('touchstart', function( e ) {
					var t = e.touches ? e.touches : e.originalEvent.touches;
					if( t.length == 1 ){
						ls.g.touchStartX = ls.g.touchEndX = t[0].clientX;
					}
			    });

			   $(el).bind('touchmove', function( e ) {
					var t = e.touches ? e.touches : e.originalEvent.touches;
					if( t.length == 1 ){
						ls.g.touchEndX = t[0].clientX;
					}
					if( Math.abs( ls.g.touchStartX - ls.g.touchEndX ) > 45 ){
						e.preventDefault();							
					}
			    });

				$(el).bind('touchend',function( e ){
					if( Math.abs( ls.g.touchStartX - ls.g.touchEndX ) > 45 ){
						if( ls.g.touchStartX - ls.g.touchEndX > 0 ){
							ls.o.cbNext();
							$(el).layerSlider('next');
						}else{
							ls.o.cbPrev();
							$(el).layerSlider('prev');
						}
					}
				});
			}
			
			// Feature: pauseOnHover (if number of layers > 1)
			
			if( ls.o.pauseOnHover == true && $(el).find('.ls-layer').length > 1 ){
				
				// BUGFIX v1.6 stop was not working because of pause on hover

				$(el).find('.ls-inner').hover(
					function(){

						// Calling cbPause callback function

						ls.o.cbPause();
						if( ls.g.autoSlideshow ){
							ls.stop();
							ls.g.paused = true;
						}
					},
					function(){
						if( ls.g.paused == true ){
							ls.start();
							ls.g.paused = false;
						}						
					}
				);
			}

			// Applying skin
			
			$(el).addClass('ls-'+ls.o.skin);
			var skinStyle = ls.o.skinsPath+ls.o.skin+'/skin.css';

			if (document.createStyleSheet){
				document.createStyleSheet(skinStyle);
			}else{
				$('<link rel="stylesheet" href="'+skinStyle+'" type="text/css" />').appendTo( $('head') );
			}				
						
			// NEW FEATURE v1.7 animating first layer

			if( ls.o.animateFirstLayer == true ){
				if( ls.o.autoStart ){
					ls.g.autoSlideshow = true;
				}
				ls.next();
			}else{

				ls.imgPreload(ls.g.curLayer,function(){
					ls.g.curLayer.fadeIn(1000, function(){
						
						$(this).addClass('ls-active');

						// NEW FEATURE v2.0 autoPlayVideos

						if( ls.o.autoPlayVideos ){
							$(this).delay( $(this).data('delayin') + 25 ).queue(function(){
								$(this).find('.ls-videopreview').click();
								$(this).dequeue();
							});							
						}
					});

					// If autoStart is true

					if( ls.o.autoStart ){
						ls.start();
					}
				});
			}
			
			// NEW FEATURE v1.7 added window resize function for make responsive layout better

			$(window).resize(function() {
				ls.makeResponsive( ls.g.curLayer, function(){return;});
			});			

			// Must be called because of Firefox, Safari and IE9 outerWidth bug

			$(el).delay(150).queue(function(){
				ls.makeResponsive( ls.g.curLayer, function(){ return; }, true);
			});

			// NEW FEATURE v1.7 added cbInit function

			ls.o.cbInit( $(el) );
		};

		ls.start = function(){

			if( ls.g.autoSlideshow ){
				if( ls.g.prevNext == 'prev' && ls.o.twoWaySlideshow ){
					ls.prev();
				}else{
					ls.next();
				}
			}else{
				ls.g.autoSlideshow = true;
				ls.timer();
			}
		};
		
		ls.timer = function(){
			
			var delaytime = $(el).find('.ls-active').data('slidedelay') ? parseInt( $(el).find('.ls-active').data('slidedelay') ) : ls.o.slideDelay;

			clearTimeout( ls.g.slideTimer );
			ls.g.slideTimer = window.setTimeout(function(){
				ls.start();
			}, delaytime );
		};

		ls.stop = function(){

			clearTimeout( ls.g.slideTimer );
			ls.g.autoSlideshow = false;
		};

		// Because of an ie7 bug, we have to check & format the strings correctly

		ls.ieEasing = function( e ){

			// BUGFIX v1.6 and v1.8 some type of animations didn't work properly

			if( $.trim(e.toLowerCase()) == 'swing' || $.trim(e.toLowerCase()) == 'linear'){
				return e.toLowerCase();
			}else{
				return e.replace('easeinout','easeInOut').replace('easein','easeIn').replace('easeout','easeOut').replace('quad','Quad').replace('quart','Quart').replace('cubic','Cubic').replace('quint','Quint').replace('sine','Sine').replace('expo','Expo').replace('circ','Circ').replace('elastic','Elastic').replace('back','Back').replace('bounce','Bounce');				
			}
		};

		// Calculating prev layer

		ls.prev = function(clicked){

			// NEW FEATURE v2.0 loops

			if( ls.g.curLayerIndex < 2 ){
				ls.g.nextLoop += 1;
			}

			if( ( ls.g.nextLoop > ls.o.loops ) && ( ls.o.loops > 0 ) && !clicked ){
				ls.g.nextLoop = 0;
				ls.stop();
				if( ls.o.forceLoopNum == false ){
					ls.o.loops = 0;						
				}
			}else{
				var prev = ls.g.curLayerIndex < 2 ? ls.g.layersNum : ls.g.curLayerIndex - 1;
				ls.g.prevNext = 'prev';
				ls.change(prev,ls.g.prevNext);
			}
		};

		// Calculating next layer

		ls.next = function(clicked){

			// NEW FEATURE v2.0 loops

			if( !(ls.g.curLayerIndex < ls.g.layersNum) ){
				ls.g.nextLoop += 1;
			}

			if( ( ls.g.nextLoop > ls.o.loops ) && ( ls.o.loops > 0 ) && !clicked ){

				ls.g.nextLoop = 0;
				ls.stop();
				if( ls.o.forceLoopNum == false ){
					ls.o.loops = 0;						
				}
			}else{

				var next = ls.g.curLayerIndex < ls.g.layersNum ? ls.g.curLayerIndex + 1 : 1;
				ls.g.prevNext = 'next';
				ls.change(next,ls.g.prevNext);
			}

		};

		ls.change = function(num,prevnext){

			// NEW FEATURE v2.0 videoPreview & autoPlayVideos

			if( ls.g.pausedByVideo == true ){

				ls.g.pausedByVideo = false;
				ls.g.autoSlideshow = ls.g.originalAutoSlideshow;
				
				ls.g.curLayer.find('iframe[src*="www.youtu"], iframe[src*="player.vimeo"]').each(function(){

					$(this).parent().find('.ls-vpcontainer').fadeIn(ls.g.v.fi,function(){
						$(this).parent().find('iframe').attr('src','');						
					});
				});
			}
			
			$(el).find('iframe[src*="www.youtu"], iframe[src*="player.vimeo"]').each(function(){
				
				// Clearing videoTimeouts
				
				clearTimeout( $(this).data( 'videoTimer') );
			});

			clearTimeout( ls.g.slideTimer );
			ls.g.nextLayerIndex = num;
			ls.g.nextLayer = $(el).find('.ls-layer:eq('+(ls.g.nextLayerIndex-1)+')');

			// BUGFIX v1.6 fixed wrong directions of animations if navigating by slidebuttons

			if( !prevnext ){

				if( ls.g.curLayerIndex < ls.g.nextLayerIndex ){
					ls.g.prevNext = 'next';
				}else{
					ls.g.prevNext = 'prev';
				}				
			}

			// Added timeOut to wait for the fade animation of videoPreview image...

			var timeOut = 0;
			
			if( $(el).find('iframe[src*="www.youtu"], iframe[src*="player.vimeo"]').length ){
				timeOut = ls.g.v.fi;
			}

			setTimeout(function() {
				ls.imgPreload(ls.g.nextLayer,function(){
					ls.animate();
				});
			}, timeOut );
		};
		
		// Preloading images

		ls.imgPreload = function(layer,callback){

			if( ls.o.imgPreload ){				
				var preImages = [];
				var preloaded = 0;

				// NEW FEATURE v1.8 Prealoading background images of layers
				
				if( layer.css('background-image') != 'none' && layer.css('background-image').indexOf('url') != -1 ){
					var bgi = layer.css('background-image');
					bgi = bgi.match(/url\((.*)\)/)[1].replace(/"/gi, '');
					preImages.push(bgi);
				}
				
				// Images inside layers

				layer.find('img').each(function(){
					preImages.push($(this).attr('src'));
				});

				// Background images inside layers

				layer.find('*').each(function(){
					
					// BUGFIX v1.7 fixed preload bug with sublayers with gradient backgrounds

					if( $(this).css('background-image') != 'none' && $(this).css('background-image').indexOf('url') != -1 ){
						var bgi = $(this).css('background-image');
						bgi = bgi.match(/url\((.*)\)/)[1].replace(/"/gi, '');
						preImages.push(bgi);
					}
				});

				// BUGFIX v1.7 if there are no images in a layer, calling the callback function

				if(preImages.length == 0){
					ls.makeResponsive(layer, callback);
				}else{
					for(x=0;x<preImages.length;x++){
						$('<img>').load(function(){
							if( ++preloaded == preImages.length ){
								ls.makeResponsive(layer, callback);
							}
						}).attr('src',preImages[x]);
					}					
				}
			}else{
				ls.makeResponsive(layer, callback);
			}
		};
		
		// NEW FEATURE v1.7 making the slider responsive

		ls.makeResponsive = function(layer, callback, bugfix){

			// BUGFIX v2.0 Fading out in forceResponsive mode
			
			if( !bugfix ){
				layer.css({
					display: 'block',
					visibility: 'hidden'
				});
			}

			ls.resizeSlider();

			for(var _sl=0;_sl < layer.children().length;_sl++){

				var sl = layer.children(':eq('+_sl+')');
				var ol = sl.data('originalLeft');
				var ot = sl.data('originalTop');

				// (RE)positioning sublayer (left property)

				if( ol && ol.indexOf('%') != -1 ){
					sl.css({
						left : ls.g.sliderWidth() / 100 * parseInt(ol) - sl.outerWidth() / 2
					});
				}

				// (RE)positioning sublayer (top property)

				if( ot && ot.indexOf('%') != -1 ){
					sl.css({
						
						// BUGFIX v2.0 fixing wrong variable

						top : ls.g.sliderHeight() / 100 * parseInt(ot) - sl.outerHeight() / 2
					});
				}
			}

			// BUGFIX v2.0 Fading out in forceResponsive mode

			if( !bugfix ){
				layer.css({
					display: 'none',
					visibility: 'visible'
				});
			}

			callback();

			$(this).dequeue();
		};
		
		// Resizing the slider

		ls.resizeSlider = function(){

			if( $(el).closest('.ls-wp-forceresponsive-container').length ){

				$(el).closest('.ls-wp-forceresponsive-helper').css({
					height : $(el).outerHeight(true)
				});

				$(el).closest('.ls-wp-forceresponsive-container').css({
					height : $(el).outerHeight(true)
				});

				$(el).closest('.ls-wp-forceresponsive-helper').css({
					width : $(window).width(),
					left : - $(window).width() / 2
				});

				if( ls.g.sliderOriginalWidth.split('%') != -1 ){

					var percentWidth = parseInt( ls.g.sliderOriginalWidth );
					var newWidth = $('body').width() / 100 * percentWidth - ( $(el).outerWidth() - $(el).width() );
					$(el).width( newWidth );
				}
			}

			$(el).find('.ls-inner').css({
				width : ls.g.sliderWidth(),
				height : ls.g.sliderHeight()
			});

			// BUGFIX v2.0 fixed width problem if firstLayer is not 1

			if( ls.g.curLayer && ls.g.nextLayer ){
				
				ls.g.curLayer.css({
					width : ls.g.sliderWidth(),
					height : ls.g.sliderHeight()
				});

				ls.g.nextLayer.css({
					width : ls.g.sliderWidth(),
					height : ls.g.sliderHeight()
				});
			}else{
				
				$(el).find('.ls-layer').css({
					width : ls.g.sliderWidth(),
					height : ls.g.sliderHeight()
				});
			}

			$(el).find('.ls-bg').css({
				marginLeft : - ( ls.g.sliderWidth() / 2 )+'px',
				marginTop : - ( ls.g.sliderHeight() / 2 )+'px'
			});
		};
		
		// Animating layers and sublayers

		ls.animate = function(){
			
			// Calling cbAnimStart callback function

			ls.o.cbAnimStart();

			// Changing variables

			ls.g.isAnimating = true;
			
			// Adding .ls-animating class to next layer
			
			ls.g.nextLayer.addClass('ls-animating');

			// Setting position and styling of current and next layers

			var curLayerLeft = curLayerRight = curLayerTop = curLayerBottom = nextLayerLeft = nextLayerRight = nextLayerTop = nextLayerBottom = layerMarginLeft = layerMarginRight = layerMarginTop = layerMarginBottom = 'auto';
			var curLayerWidth = nextLayerWidth = ls.g.sliderWidth();
			var curLayerHeight = nextLayerHeight = ls.g.sliderHeight();

			// Calculating direction

			var prevOrNext = ls.g.prevNext == 'prev' ? ls.g.curLayer : ls.g.nextLayer;
			var chooseDirection = prevOrNext.data('slidedirection') ? prevOrNext.data('slidedirection') : ls.o.slideDirection;

			// Setting the direction of sliding

			var slideDirection = ls.g.slideDirections[ls.g.prevNext][chooseDirection];

			if( slideDirection == 'left' || slideDirection == 'right' ){
				curLayerWidth = curLayerTop = nextLayerWidth = nextLayerTop = 0;
				layerMarginTop = 0;				
			}
			if( slideDirection == 'top' || slideDirection == 'bottom' ){
				curLayerHeight = curLayerLeft = nextLayerHeight = nextLayerLeft = 0;
				layerMarginLeft = 0;
			}

			switch(slideDirection){
				case 'left':
					curLayerRight = nextLayerLeft = 0;
					layerMarginLeft = -ls.g.sliderWidth();
					break;
				case 'right':
					curLayerLeft = nextLayerRight = 0;
					layerMarginLeft = ls.g.sliderWidth();
					break;
				case 'top':
					curLayerBottom = nextLayerTop = 0;
					layerMarginTop = -ls.g.sliderHeight();
					break;
				case 'bottom':
					curLayerTop = nextLayerBottom = 0;
					layerMarginTop = ls.g.sliderHeight89;
					break;
			}

			// Setting start positions and styles of layers

			ls.g.curLayer.css({
				left : curLayerLeft,
				right : curLayerRight,
				top : curLayerTop,
				bottom : curLayerBottom			
			});
			ls.g.nextLayer.css({
				width : nextLayerWidth,
				height : nextLayerHeight,
				left : nextLayerLeft,
				right : nextLayerRight,
				top : nextLayerTop,
				bottom : nextLayerBottom
			});

			// Animating current layer

			// BUGFIX v1.6 fixed some wrong parameters of current layer
			// BUGFIX v1.7 fixed using of delayout of current layer

			var curDelay = ls.g.curLayer.data('delayout') ? parseInt(ls.g.curLayer.data('delayout')) : ls.o.delayOut;
			var curTime = ls.g.curLayer.data('durationout') ? parseInt(ls.g.curLayer.data('durationout')) : ls.o.durationOut;
			var curEasing = ls.g.curLayer.data('easingout') ? ls.g.curLayer.data('easingout') : ls.o.easingOut;

			// BUGFIX v1.6 added an additional delaytime to current layer to fix the '1px gap' bug
			// BUGFIX v2.0 modified from curTime / 80 to curTime / 120

			ls.g.curLayer.delay( curDelay + curTime / 80 ).animate({
				width : curLayerWidth,
				height : curLayerHeight
			}, curTime, curEasing,function(){

				// Setting current layer

				ls.g.curLayer = ls.g.nextLayer;
				ls.g.curLayerIndex = ls.g.nextLayerIndex;

				// Changing some css classes

				$(el).find('.ls-layer').removeClass('ls-active');
				$(el).find('.ls-layer:eq(' + ( ls.g.curLayerIndex - 1 ) + ')').addClass('ls-active').removeClass('ls-animating');
				$(el).find('.ls-bottom-slidebuttons a').removeClass('ls-nav-active');
				$(el).find('.ls-bottom-slidebuttons a:eq('+( ls.g.curLayerIndex - 1 )+')').addClass('ls-nav-active');
			
				// Changing variables

				ls.g.isAnimating = false;

				// Calling cbAnimStop callback function

				ls.o.cbAnimStop();

				// Setting timer if needed

				if( ls.g.autoSlideshow ){

					ls.timer();
				}
			});

			// Animating sublayers of current layer

			ls.g.curLayer.find(' > *[class*="ls-s"]').each(function(){

				var curSubSlideDir = $(this).data('slidedirection') ? $(this).data('slidedirection') : slideDirection;
				var lml, lmt;

				switch(curSubSlideDir){
					case 'left':
						lml = -ls.g.sliderWidth();
						lmt = 0;
						break;
					case 'right':
						lml = ls.g.sliderWidth();
						lmt = 0;
						break;
					case 'top':
						lmt = -ls.g.sliderHeight();
						lml = 0;
						break;
					case 'bottom':
						lmt = ls.g.sliderHeight();
						lml = 0;
						break;
				}

				// NEW FEATURE v1.6 added slideoutdirection to sublayers

				var curSubSlideOutDir = $(this).data('slideoutdirection') ? $(this).data('slideoutdirection') : false;

				switch(curSubSlideOutDir){
					case 'left':
						lml = ls.g.sliderWidth();
						lmt = 0;
						break;
					case 'right':
						lml = -ls.g.sliderWidth();
						lmt = 0;
						break;
					case 'top':
						lmt = ls.g.sliderHeight();
						lml = 0;
						break;
					case 'bottom':
						lmt = -ls.g.sliderHeight();
						lml = 0;
						break;
				}

				var curSubParMod = ls.g.curLayer.data('parallaxout') ? parseInt(ls.g.curLayer.data('parallaxout')) : ls.o.parallaxOut;
				var curSubPar = parseInt( $(this).attr('class').split('ls-s')[1] ) * curSubParMod;
				
				var curSubDelay = $(this).data('delayout') ? parseInt($(this).data('delayout')) : ls.o.delayOut;
				var curSubTime = $(this).data('durationout') ? parseInt($(this).data('durationout')) : ls.o.durationOut;
				var curSubEasing = $(this).data('easingout') ? $(this).data('easingout') : ls.o.easingOut;

				// NEW FEATURE v1.6 added fading feature to sublayers

				if( curSubSlideOutDir == 'fade' || ( !curSubSlideOutDir && curSubSlideDir == 'fade' )){
					
					$(this).delay( curSubDelay ).fadeOut(curSubTime, curSubEasing);					
				}else{
					
					$(this).stop().delay( curSubDelay ).animate({
						marginLeft : -lml * curSubPar,
						marginTop : -lmt * curSubPar
					}, curSubTime, curSubEasing);
				}
			});	

			// Animating next layer

				// Replacing global parameters with unique if need

				var nextDelay = ls.g.nextLayer.data('delayin') ? parseInt(ls.g.nextLayer.data('delayin')) : ls.o.delayIn;
				var nextTime = ls.g.nextLayer.data('durationin') ? parseInt(ls.g.nextLayer.data('durationin')) : ls.o.durationIn;
				var nextEasing = ls.g.nextLayer.data('easingin') ? ls.g.nextLayer.data('easingin') : ls.o.easingIn;

				ls.g.nextLayer.delay( curDelay + nextDelay ).animate({
					width : ls.g.sliderWidth(),
					height : ls.g.sliderHeight()
				}, nextTime, nextEasing);

			// Animating sublayers of next layer

			ls.g.nextLayer.find(' > *[class*="ls-s"]').each(function(){

				// Replacing global parameters with unique if need

				var nextSubSlideDir = $(this).data('slidedirection') ? $(this).data('slidedirection') : slideDirection;
				var lml, lmt;

				switch(nextSubSlideDir){
					case 'left':
						lml = -ls.g.sliderWidth();
						lmt = 0;
						break;
					case 'right':
						lml = ls.g.sliderWidth();
						lmt = 0;
						break;
					case 'top':
						lmt = -ls.g.sliderHeight();
						lml = 0;
						break;
					case 'bottom':
						lmt = ls.g.sliderHeight();
						lml = 0;
						break;
					case 'fade':
						lmt = 0;
						lml = 0;
						break;
				}

				var nextSubParMod = ls.g.nextLayer.data('parallaxin') ? parseInt(ls.g.nextLayer.data('parallaxin')) : ls.o.parallaxIn;
				var nextSubPar = parseInt( $(this).attr('class').split('ls-s')[1] ) * nextSubParMod;

				var nextSubDelay = $(this).data('delayin') ? parseInt($(this).data('delayin')) : ls.o.delayIn;
				var nextSubTime = $(this).data('durationin') ? parseInt($(this).data('durationin')) : ls.o.durationIn;
				var nextSubEasing = $(this).data('easingin') ? $(this).data('easingin') : ls.o.easingIn;

				// NEW FEATURE v1.6 added fading feature to sublayers

				if( nextSubSlideDir == 'fade' ){
					
					$(this).css({
						display: 'none',
						marginLeft : 0,
						marginTop : 0
					}).delay( curDelay + nextSubDelay ).fadeIn(nextSubTime, nextSubEasing, function(){
						
						// NEW FEATURE v2.0 autoPlayVideos

						if( ls.o.autoPlayVideos == true ){

							$(this).find('.ls-videopreview').click();
						}					
					});
				}else{

					// BUGFIX v1.7 added display : block to sublayers that don't fade

					$(this).css({
						display : 'block',
						marginLeft : lml * nextSubPar,
						marginTop : lmt * nextSubPar
					}).stop().delay( curDelay + nextSubDelay ).animate({
						marginLeft : 0,
						marginTop : 0
					}, nextSubTime, nextSubEasing, function(){
						
						// NEW FEATURE v2.0 autoPlayVideos

						if( ls.o.autoPlayVideos == true ){

							$(this).find('.ls-videopreview').click();
						}					
					});
				}				
			});
		}

		// initializing
		ls.init();
	};

	layerSlider.options = {
		
		// User settings (can be modified)
		
		autoStart			: true,						// If true, slideshow will automatically start after loading the page.
		firstLayer			: 1,						// LayerSlider will begin with this layer.
		twoWaySlideshow		: false,					// If true, slideshow will go backwards if you click the prev button.
		keybNav				: true,						// Keyboard navigation. You can navigate with the left and right arrow keys.

		// NEW FEATURES v2.0 touchNav
		
		touchNav			: true,						// Touch-control (on mobile devices)

		imgPreload			: true,						// Image preload. Preloads all images and background-images of the next layer.
		navPrevNext			: true,						// If false, Prev and Next buttons will be invisible.
		navStartStop		: true,						// If false, Start and Stop buttons will be invisible.
		navButtons			: true,						// If false, slide buttons will be invisible.
		skin				: 'lightskin',				// You can change the skin of the Slider, use 'noskin' to hide skin and buttons. (Pre-defined skins are: 'deafultskin', 'lightskin', 'darkskin', 'glass' and 'minimal'.)
		skinsPath			: '/layerslider/skins/',	// You can change the default path of the skins folder. Note, that you must use the slash at the end of the path.
		pauseOnHover		: true,						// SlideShow will pause when mouse pointer is over LayerSlider.

		// NEW FEATURES v1.6 optional globalBGColor & globalBGImage

		globalBGColor		: 'transparent',			// Background color of LayerSlider. You can use all CSS methods, like hexa colors, rgb(r,g,b) method, color names, etc. Note, that background sublayers are covering the background.
		globalBGImage		: false,					// Background image of LayerSlider. This will be a fixed background image of LayerSlider by default. Note, that background sublayers are covering the global background image.

		// NEW FEATURES v1.7 animateFirstLayer & yourLogo

		animateFirstLayer	: false,					// If true, first layer will animate (slide in) instead of fading
		yourLogo			: false,					// This is a fixed image that will be shown above of LayerSlider container. For example if you want to display your own logo, etc. You have to add the correct path to your image file.
		yourLogoStyle		: 'position: absolute; z-index: 1001; left: 10px; top: 10px;', // You can style your logo. You are allowed to use any CSS properties, for example add left and top properties to place the image inside the LayerSlider container anywhere you want.

		// NEW FEATURES v1.8 yourLogoLink & yourLogoTarget
		
		yourLogoLink		: false,					// You can add a link to your logo. Set false is you want to display only an image without a link.
		yourLogoTarget		: '_blank',					// If '_blank', the clicked url will open in a new window.

		// NEW FEATURES v2.0 loops, forceLoopNum, autoPlayVideos, autoPauseSlideshow & youtubePreview
		
		loops				: 0,						// Number of loops if autoStart set true (0 means infinite!)
		forceLoopNum		: true,						// If true, the slider will always stop at the given number of loops even if the user restarts the slideshow
		autoPlayVideos		: true,						// If true, slider will autoplay youtube / vimeo videos - you can use it with autoPauseSlideshow
		autoPauseSlideshow	: 'auto',					// 'auto', true or false. 'auto' means, if autoPlayVideos is set to true, slideshow will stop UNTIL the video is playing and after that it continues. True means slideshow will stop and it won't continue after video is played.
		youtubePreview		: 'maxresdefault.jpg',		// Default thumbnail picture of YouTube videos. Can be 'maxresdefault.jpg', 'hqdefault.jpg', 'mqdefault.jpg' or 'default.jpg'. Note, that 'maxresdefault.jpg' os not available to all (not HD) videos.

		// LayerSlider API callback functions

		cbInit				: function(){},				// Calling when LayerSlider loads.
		cbStart				: function(){},				// Calling when you click the slideshow start button.
		cbStop				: function(){},				// Calling when click the slideshow stop / pause button.
		cbPause				: function(){},				// Calling when slideshow pauses (if pauseOnHover is true).
		cbAnimStart			: function(){},				// Calling when animation starts.
		cbAnimStop			: function(){},				// Calling when the animation of current layer ends, but the sublayers of this layer still may be animating.
		cbPrev				: function(){},				// Calling when you click the previous button (or if you use keyboard or touch navigation).
		cbNext				: function(){},				// Calling when you click the next button (or if you use keyboard or touch navigation).

		// The following global settings can be override separately by each layers and / or sublayers local settings (see the documentation for more information).
		
		slideDirection		: 'right',					// Slide direction. New layers will sliding FROM(!) this direction.
		slideDelay			: 4000,						// Time before the next slide will be loading.
		parallaxIn			: .45,						// Modifies the parallax-effect of the slide-in animation.
		parallaxOut			: .45,						// Modifies the parallax-effect of the slide-out animation.
		durationIn			: 1500,						// Duration of the slide-in animation.
		durationOut			: 1500,						// Duration of the slide-out animation.
		easingIn			: 'easeInOutQuint',			// Easing (type of transition) of the slide-in animation.
		easingOut			: 'easeInOutQuint',			// Easing (type of transition) of the slide-out animation.
		delayIn				: 0,						// Delay time of the slide-in animation.
		delayOut			: 0							// Delay time of the slide-out animation.
	};

	layerSlider.global = {
		
		// Global parameters (Do not change these settings!)

		version				: '2.0',
		
		paused				: false,
		autoSlideshow		: false,
		isAnimating			: false,
		layersNum			: null,
		prevNext			: 'next',
		slideTimer			: null,
		sliderWidth			: null,
		sliderHeight		: null,
		slideDirections		: {
								prev : {
									left	: 'right',
									right	: 'left',
									top		: 'bottom',
									bottom	: 'top'
								},
								next : {
									left	: 'left',
									right	: 'right',
									top		: 'top',
									bottom	: 'bottom'
								}
							},

		// Default delay time, fadeout and fadein durations of videoPreview images

		v					: {
								d	: 500,
								fo	: 750,
								fi	: 500	
							}
	};

})(jQuery);