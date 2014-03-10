ContentManager v.90 preview.


Planned Future Features.
-write a Add/Edit item page.  This page will be used to add support for modules.  Adding
an item will run the sql to add sprocs for that module, editing allows changing names,etc.
Accept input from xml templates also!  Bulk imports done via sql.

-installer checks to see if module datatable has been modified in format before installing
support.  if they delete a field from rb_Announcements then the install should notify with
an error.

Known Issues.
-workflow!!!!  You cannot moved published modules on workflow enabled modules.
Can fix this by adding new entry to rb_ContentManager for workflow modules.
You can move between staging areas.


-(confusing, sorry)If you have only ONE module instance of the selected type within a single portal.
AND you select that module type/portal in both the source portal and destination portal,
whichever portal(source or destination) you picked second will show ZERO module instances,
this is correct.  However, if you then change either portal instance, it will NOT reload
the moduleinstances for the other portal.

*************************Changes in New Version  11/8/2004**********************************
by Manu
- Cleaned the install process. Now it is all automatic, simply put your new module script 
  on InstallScript folder and install the module again. 
  names MUST end as _install.sql or _unistall.sql
- Changed to admin module
- Cahged name: now it names "Admin - Content Manager"
- Now install and uninstall drops content manager table. 
  It is important because installing twice occurs in duplicate modules entries.
- Fixed task support: ModuleId filed was duplicated
- Fixed task support: Proc names were stored in wrong way
- Fixed blog support: Wrong guid in installation


*************************Changes in New Version  1/19/04-1/20/04**********************************
Added support for
Tasks module
Blog Module

*************************Changes in New Version  1/19/04-1/20/04**********************************
1:  Moved ModuleTypes to a row all by itself above everything.  Makes UI alignment a bit
more clear for the user to know how the module works.
2:  Changed column widths from 33%/33%/33% to 43%/16%/43% so that items in listboxes are wider.
3:  Added SourcePortal/Destination Portal dropdownlists to allow moving items between portals.
NOTE:  the previous versions included ALL modules from ALL portals and I just didn't notice
because I only run one portal instance on my development machine.
4:  Added SettingItem for ShowMultiplePortals.  This way super admins can disable the
collection properties for lower level admins(of a portal instance perhaps) and disable
multi-portal support.  Result == single site admin cannot change other sites in same
database.



*************************ContentManager-ALPHA2 1/4/04*************************************
TODO:
1:  Mark source modules with no items in them, either by color or adding a counter or something.
2:  Delete confirmation via javascript





HOW TO ADD support for a module not already supported.

Procs you need to write.
1:  Get Summary
       PURPOSE:  Short description of the item, sufficient for them to identify the item.
       INPUT PARAMS:
            ModuleID the ModuleID of the instance you wish to show a summary for.
       RETURNS:  ItemID, ItemDesc
       TIPS
            -Use concatenation(+ operator) to combine fields for ItemDesc if a single field
                cannot adequately describe to the admin which item it is.  Be sure to
                alias this field so that it returns a field named ItemDesc.
            -Use LEFT(fieldname, length) to trim fields.

(use an alias and concatenate fields)

2:  CopyItem
       PURPOSE:  Copies a record in the module's table setting the ModuleID of the new
           record to a given value and thus copying it.
       INPUT PARAMS:
            ItemID          the unique identifier of the item you wish to copy.
            TargetModuleID  The module you wish to copy that item to.
       TIPS:  You can copy and paste the INSERT sproc for the selected item to get the
          first half of the statement.  Re-copy all but the ModuleID from what you just
          copied for the rest.

3:  MoveItem
       PURPOSE:  moves a record from one module instance to another(changes ModuleID).
       INPUT PARAMS:
            ItemID          the unique identifier of the item to move.
            TargetModuleID  where to move it to.
4:  CopyAll
       PURPOSE:  copy all records from one module instance to another.  Does not over-write,
        replace or remove existing records, it simply ADDS to those.
       INPUT PARAMS:
            SourceModuleID      copy from here.
            TargetModuleID      copy TO here.
       TIPS:  You can pretty much copy the CopyItem sproc and just change the IN params,
        and the WHERE clause(change ItemID to ModuleID)

5:  Insert the record into the ContentManager table.
        ItemID                  -used for simpler coding in the ascx and DAL.  Easier to
                                 deal with a int than a GUID.
        GeneralModDefID         -This is the GUID from rb_GeneralModuleDefinitions.GeneralDefModId.
                                  used to reduce the number of joins by 1(no join required to generalmoduledefinitons table)
        FriendlyName            -whatever you want to call it.  I recommend the same as in rb_GeneralModuleDefinitions
        SummarySproc            -the name you gave for the sproc in step #1
        CopyItemSproc           -the name you gave for the sproc in step #2
        MoveItemSproc           -the name you gave for the sproc in step #3
        CopyAllSproc            -the name you gave for the sproc in step #4
        DeleteItemSproc         -YOU DO NOT CREATE THIS, it should already exist.  This is the
                                 name of the modules' delete function which is re-used here.
                                 For Announcements it was rb_DeleteAnnouncement, Articles was
                                 rb_DeleteArticle.

