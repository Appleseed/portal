namespace Appleseed.Framework.Configuration.Items
{
    using System;

    /// <summary>
    /// Slider Image Details
    /// </summary>
    public class SliderImageItem
    {
        /// <summary>
        /// Slider Image ID
        /// </summary>
        public int SliderImageID { get; set; }

        /// <summary>
        /// Slider ID
        /// </summary>
        public int SliderID { get; set; }

        /// <summary>
        /// Slider Caption
        /// </summary>
        public string SliderCaption { get; set; }

        /// <summary>
        /// Slider Image Extension 
        /// </summary>
        public string SliderImageExt { get; set; }

        /// <summary>
        /// Slider Image Created Date
        /// </summary>
        public DateTime CreatedDate { get; set; }

        /// <summary>
        /// Slider Image Created By User Name
        /// </summary>
        public string CreatedUserName { get; set; }

        /// <summary>
        /// Slider Image Updated Date
        /// </summary>
        public DateTime UpdatedDate { get; set; }

        /// <summary>
        /// Slider Image Updated By UserName
        /// </summary>
        public string UpdatedUserName { get; set; }

        /// <summary>
        /// Slider Image is Active or not
        /// </summary>
        public bool Active { get; set; }

        /// <summary>
        /// Slider Image Display Order
        /// </summary>
        public int DisplayOrder { get; set; }
    }
}
