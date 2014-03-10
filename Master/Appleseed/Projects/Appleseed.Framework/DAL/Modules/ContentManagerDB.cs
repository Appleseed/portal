using System.Data;
using System.Data.SqlClient;

using Appleseed.Framework.Settings;
using Appleseed.Framework.Data;
using System.Linq;
using Appleseed.Framework.Models;
namespace Appleseed.Framework.Content.Data
{
    /// <summary>
    /// Summary description for ContentManager.
    /// </summary>
    public class ContentManagerDB {

        /// <summary>
        /// Gets the module types.
        /// </summary>
        /// <returns></returns>
        public SqlDataReader GetModuleTypes()
        {
            //  Create Instance of Connection and Command Object
			SqlConnection myConnection = Config.SqlConnectionString;
			SqlCommand myCommand = new SqlCommand("rb_ContentMgr_GetModuleTypes", myConnection);
            myCommand.CommandType = CommandType.StoredProcedure;

            //  Execute the command
            myConnection.Open();
            return myCommand.ExecuteReader(CommandBehavior.CloseConnection);

        }
        /// <summary>
        /// Gets the portals.
        /// </summary>
        /// <returns></returns>
        public SqlDataReader GetPortals()
        {
            //  Create Instance of Connection and Command Object
			SqlConnection myConnection = Config.SqlConnectionString;
			SqlCommand myCommand = new SqlCommand("rb_ContentMgr_GetPortals", myConnection);
            myCommand.CommandType = CommandType.StoredProcedure;

            //  Execute the command
            myConnection.Open();
            return myCommand.ExecuteReader(CommandBehavior.CloseConnection);

        }
        /// <summary>
        /// Gets the module instances.
        /// </summary>
        /// <param name="ItemID">The item ID.</param>
        /// <param name="PortalID">The portal ID.</param>
        /// <returns></returns>
        public IQueryable GetModuleInstances(int ItemID, int PortalID)
        {
            var ctx = new AppleseedDBContext();

            var result = (from pages in ctx.rb_Pages
                          from modules in ctx.rb_Modules
                          from moduleDefs in ctx.rb_ModuleDefinitions
                          from contMan in ctx.rb_ContentManager

                          where contMan.ItemID == ItemID &&
                                contMan.SourceGeneralModDefID == moduleDefs.GeneralModDefID &&
                                moduleDefs.ModuleDefID == modules.ModuleDefID &&
                                moduleDefs.PortalID == PortalID &&
                                modules.TabID == pages.PageID

                          select new
                          {
                              modules.ModuleID,
                              TabModule = pages.PageName + "\\" + modules.ModuleTitle
                          }

                ).AsQueryable().OrderBy(r => r.TabModule);

            return result;
        }

        /// <summary>
        /// Gets the module instances exc.
        /// </summary>
        /// <param name="ItemID">The item ID.</param>
        /// <param name="ExcludeItem">The exclude item.</param>
        /// <param name="PortalID">The portal ID.</param>
        /// <returns></returns>
        /// ExcludeItem == ModuleID of selected Module to use for source.
        //Since we do not want to allow copy from A->A,B->B we exclude that item.
        public IQueryable GetModuleInstancesExc(int ItemID, int ExcludeItem, int PortalID)
        {
            var ctx = new AppleseedDBContext();

            var result = (from pages in ctx.rb_Pages
                          from modules in ctx.rb_Modules
                          from moduleDefs in ctx.rb_ModuleDefinitions
                          from contMan in ctx.rb_ContentManager

                          where contMan.ItemID == ItemID &&
                                contMan.ItemID != ExcludeItem &&
                                contMan.SourceGeneralModDefID == moduleDefs.GeneralModDefID &&
                                moduleDefs.ModuleDefID == modules.ModuleDefID &&
                                moduleDefs.PortalID == PortalID &&
                                modules.TabID == pages.PageID

                          select new
                          {
                              modules.ModuleID,
                              TabModule = pages.PageName + "\\" + modules.ModuleTitle
                          }                          

                ).AsQueryable().OrderBy(r => r.TabModule);

            return result;
        }

        /// <summary>
        /// Gets the source module data.
        /// </summary>
        /// <param name="ContentMgr_ItemID">The content MGR_ item ID.</param>
        /// <param name="ModuleID">The module ID.</param>
        /// <returns></returns>
        public SqlDataReader GetSourceModuleData(int ContentMgr_ItemID,int ModuleID)
        {
            // Create Instance of Connection and Command Object
            SqlConnection myConnection = Config.SqlConnectionString;
            SqlCommand myCommand = new SqlCommand("rb_ContentMgr_GetSourceModuleData", myConnection);

            // Mark the Command as a SPROC
            myCommand.CommandType = CommandType.StoredProcedure;

            SqlParameter parameterModuleID = new SqlParameter("@ModuleID", SqlDbType.Int,4);
            parameterModuleID.Value = ModuleID;
            myCommand.Parameters.Add(parameterModuleID);

            SqlParameter parameterContentMgr_ItemID = new SqlParameter("@ContentMgr_ItemID", SqlDbType.Int,4);
            parameterContentMgr_ItemID.Value = ContentMgr_ItemID;
            myCommand.Parameters.Add(parameterContentMgr_ItemID);

            myConnection.Open();
            return myCommand.ExecuteReader(CommandBehavior.CloseConnection);
        }

        /// <summary>
        /// Gets the dest module data.
        /// </summary>
        /// <param name="ContentMgr_ItemID">The content MGR_ item ID.</param>
        /// <param name="ModuleID">The module ID.</param>
        /// <returns></returns>
		public SqlDataReader GetDestModuleData(int ContentMgr_ItemID,int ModuleID)
		{
			// Create Instance of Connection and Command Object
			SqlConnection myConnection = Config.SqlConnectionString;
			SqlCommand myCommand = new SqlCommand("rb_ContentMgr_GetDestModuleData", myConnection);

			// Mark the Command as a SPROC
			myCommand.CommandType = CommandType.StoredProcedure;

			SqlParameter parameterModuleID = new SqlParameter("@ModuleID", SqlDbType.Int,4);
			parameterModuleID.Value = ModuleID;
			myCommand.Parameters.Add(parameterModuleID);

			SqlParameter parameterContentMgr_ItemID = new SqlParameter("@ContentMgr_ItemID", SqlDbType.Int,4);
			parameterContentMgr_ItemID.Value = ContentMgr_ItemID;
			myCommand.Parameters.Add(parameterContentMgr_ItemID);

			myConnection.Open();
			return myCommand.ExecuteReader(CommandBehavior.CloseConnection);
		}


        /// <summary>
        /// Moves the item left.
        /// </summary>
        /// <param name="ContentMgr_ItemID">The content MGR_ item ID.</param>
        /// <param name="TargetItemID">The target item ID.</param>
        /// <param name="TargetModuleID">The target module ID.</param>
		public void MoveItemLeft(int ContentMgr_ItemID,int TargetItemID,int TargetModuleID)
        {
            //  Create Instance of Connection and Command Object
			SqlConnection myConnection = Config.SqlConnectionString;
			SqlCommand myCommand = new SqlCommand("rb_ContentMgr_MoveItemLeft", myConnection);
            myCommand.CommandType = CommandType.StoredProcedure;

            //  Add Parameters to SPROC
            SqlParameter parameterContentMgr_ItemID = new SqlParameter("@ContentMgr_ItemID", SqlDbType.Int, 4);
            parameterContentMgr_ItemID.Value = ContentMgr_ItemID;
            myCommand.Parameters.Add(parameterContentMgr_ItemID);

            SqlParameter parameterItemID = new SqlParameter("@ItemID", SqlDbType.Int, 4);
            parameterItemID.Value = TargetItemID;
            myCommand.Parameters.Add(parameterItemID);

            SqlParameter parameterTargetModuleID = new SqlParameter("@TargetModuleID", SqlDbType.Int, 4);
            parameterTargetModuleID.Value = TargetModuleID;
            myCommand.Parameters.Add(parameterTargetModuleID);

            myConnection.Open();
            myCommand.ExecuteNonQuery();
            myConnection.Close();
        }
        /// <summary>
        /// Moves the item right.
        /// </summary>
        /// <param name="ContentMgr_ItemID">The content MGR_ item ID.</param>
        /// <param name="TargetItemID">The target item ID.</param>
        /// <param name="TargetModuleID">The target module ID.</param>
		public void MoveItemRight(int ContentMgr_ItemID,int TargetItemID,int TargetModuleID)
		{
			//  Create Instance of Connection and Command Object
			SqlConnection myConnection = Config.SqlConnectionString;
			SqlCommand myCommand = new SqlCommand("rb_ContentMgr_MoveItemRight", myConnection);
			myCommand.CommandType = CommandType.StoredProcedure;

			//  Add Parameters to SPROC
			SqlParameter parameterContentMgr_ItemID = new SqlParameter("@ContentMgr_ItemID", SqlDbType.Int, 4);
			parameterContentMgr_ItemID.Value = ContentMgr_ItemID;
			myCommand.Parameters.Add(parameterContentMgr_ItemID);

			SqlParameter parameterItemID = new SqlParameter("@ItemID", SqlDbType.Int, 4);
			parameterItemID.Value = TargetItemID;
			myCommand.Parameters.Add(parameterItemID);

			SqlParameter parameterTargetModuleID = new SqlParameter("@TargetModuleID", SqlDbType.Int, 4);
			parameterTargetModuleID.Value = TargetModuleID;
			myCommand.Parameters.Add(parameterTargetModuleID);

			myConnection.Open();
			myCommand.ExecuteNonQuery();
			myConnection.Close();
		}

        /// <summary>
        /// Copies the item.
        /// </summary>
        /// <param name="ContentMgr_ItemID">The content MGR_ item ID.</param>
        /// <param name="ItemID">The item ID.</param>
        /// <param name="TargetModuleID">The target module ID.</param>
        public void CopyItem(int ContentMgr_ItemID,int ItemID, int TargetModuleID)
        {
            //  Create Instance of Connection and Command Object
			SqlConnection myConnection = Config.SqlConnectionString;
			SqlCommand myCommand = new SqlCommand("rb_ContentMgr_CopyItem", myConnection);
            myCommand.CommandType = CommandType.StoredProcedure;

            //  Add Parameters to SPROC
            SqlParameter parameterContentMgr_ItemID = new SqlParameter("@ContentMgr_ItemID", SqlDbType.Int, 4);
            parameterContentMgr_ItemID.Value = ContentMgr_ItemID;
            myCommand.Parameters.Add(parameterContentMgr_ItemID);

            SqlParameter parameterItemID = new SqlParameter("@ItemID", SqlDbType.Int, 4);
            parameterItemID.Value = ItemID;
            myCommand.Parameters.Add(parameterItemID);

            SqlParameter parameterTargetModuleID = new SqlParameter("@TargetModuleID", SqlDbType.Int, 4);
            parameterTargetModuleID.Value = TargetModuleID;
            myCommand.Parameters.Add(parameterTargetModuleID);

            myConnection.Open();
            myCommand.ExecuteNonQuery();
            myConnection.Close();

        }
        /// <summary>
        /// Copies all.
        /// </summary>
        /// <param name="ContentMgr_ItemID">The content MGR_ item ID.</param>
        /// <param name="SourceModuleID">The source module ID.</param>
        /// <param name="TargetModuleID">The target module ID.</param>
        public void CopyAll(int ContentMgr_ItemID,int SourceModuleID, int TargetModuleID)
        {
            //  Create Instance of Connection and Command Object
			SqlConnection myConnection = Config.SqlConnectionString;
			SqlCommand myCommand = new SqlCommand("rb_ContentMgr_CopyAll", myConnection);
            myCommand.CommandType = CommandType.StoredProcedure;

            //  Add Parameters to SPROC
            SqlParameter parameterContentMgr_ItemID = new SqlParameter("@ContentMgr_ItemID", SqlDbType.Int, 4);
            parameterContentMgr_ItemID.Value = ContentMgr_ItemID;
            myCommand.Parameters.Add(parameterContentMgr_ItemID);

            SqlParameter parameterSourceModuleID = new SqlParameter("@SourceModuleID", SqlDbType.Int, 4);
            parameterSourceModuleID.Value = SourceModuleID;
            myCommand.Parameters.Add(parameterSourceModuleID);

            SqlParameter parameterTargetModuleID = new SqlParameter("@TargetModuleID", SqlDbType.Int, 4);
            parameterTargetModuleID.Value = TargetModuleID;
            myCommand.Parameters.Add(parameterTargetModuleID);

            myConnection.Open();
            myCommand.ExecuteNonQuery();
            myConnection.Close();
        }
        /// <summary>
        /// Deletes the item left.
        /// </summary>
        /// <param name="ContentMgr_ItemID">The content MGR_ item ID.</param>
        /// <param name="ItemID">The item ID.</param>
        public void DeleteItemLeft(int ContentMgr_ItemID,int ItemID)
        {
            //  Create Instance of Connection and Command Object
			SqlConnection myConnection = Config.SqlConnectionString;
			SqlCommand myCommand = new SqlCommand("rb_ContentMgr_DeleteItemLeft", myConnection);
            myCommand.CommandType = CommandType.StoredProcedure;

            //  Add Parameters to SPROC
            SqlParameter parameterContentMgr_ItemID = new SqlParameter("@ContentMgr_ItemID", SqlDbType.Int, 4);
            parameterContentMgr_ItemID.Value = ContentMgr_ItemID;
            myCommand.Parameters.Add(parameterContentMgr_ItemID);

            SqlParameter parameterItemID = new SqlParameter("@ItemID", SqlDbType.Int, 4);
            parameterItemID.Value = ItemID;
            myCommand.Parameters.Add(parameterItemID);

            myConnection.Open();
            myCommand.ExecuteNonQuery();
            myConnection.Close();
        }
        /// <summary>
        /// Deletes the item right.
        /// </summary>
        /// <param name="ContentMgr_ItemID">The content MGR_ item ID.</param>
        /// <param name="ItemID">The item ID.</param>
		public void DeleteItemRight(int ContentMgr_ItemID,int ItemID)
		{
			//  Create Instance of Connection and Command Object
			SqlConnection myConnection = Config.SqlConnectionString;
			SqlCommand myCommand = new SqlCommand("rb_ContentMgr_DeleteItemRight", myConnection);
			myCommand.CommandType = CommandType.StoredProcedure;

			//  Add Parameters to SPROC
			SqlParameter parameterContentMgr_ItemID = new SqlParameter("@ContentMgr_ItemID", SqlDbType.Int, 4);
			parameterContentMgr_ItemID.Value = ContentMgr_ItemID;
			myCommand.Parameters.Add(parameterContentMgr_ItemID);

			SqlParameter parameterItemID = new SqlParameter("@ItemID", SqlDbType.Int, 4);
			parameterItemID.Value = ItemID;
			myCommand.Parameters.Add(parameterItemID);

			myConnection.Open();
			myCommand.ExecuteNonQuery();
			myConnection.Close();
		}
    }//end class
}//end namespace
