// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SliderDB.cs">
//   Copyright © -- 2015. All Rights Reserved.
// </copyright>
// <summary>
//   SliderDB class
// </summary>
//   <company>
//   HaptiX
//   </company>
// --------------------------------------------------------------------------------------------------------------------
namespace Appleseed.Framework.Site.Data
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Data.SqlClient;
    using System.Linq;
    using Appleseed.Framework.Configuration.Items;
    using Appleseed.Framework.Settings;

    /// <summary>
    /// Slider 
    /// </summary>
    [History("Ashish.patel@haptix.biz", "2015/02/10", "Class for sliders")]
    public class SliderDB
    {
        /// <summary>
        /// Get all sliders from DB
        /// </summary>
        /// <returns>Slider List</returns>
        public List<SliderItem> AllSliders(int moduleId)
        {
            return Appleseed.Framework.Data.DBHelper.DataReaderToObjectList<SliderItem>("SELECT * from rb_Sliders where ModuleId = " + moduleId);
        }

        /// <summary>
        /// "Update Slider 
        /// </summary>
        /// <param name="sliderItem">object of SliderItem</param>
        public void UpdateSlider(SliderItem item)
        {
            using (var connection = Config.SqlConnectionString)
            using (var sqlCommand = new SqlCommand("update rb_sliders SET ClientWorkPosition = @ClientWorkPosition, ClientQuote = @ClientQuote, ClientFirstName = @ClientFirstName, ClientLastName = @ClientLastName where Id = @Id", connection))
            {
                // Mark the Command as a SPROC
                sqlCommand.CommandType = CommandType.Text;

                sqlCommand.Parameters.Add(new SqlParameter("@Id", SqlDbType.Int) { Value = item.Id });
                sqlCommand.Parameters.Add(new SqlParameter("@ClientQuote", SqlDbType.NVarChar, 2000) { Value = item.ClientQuote });
                sqlCommand.Parameters.Add(new SqlParameter("@ClientFirstName", SqlDbType.NVarChar, 100) { Value = item.ClientFirstName });
                sqlCommand.Parameters.Add(new SqlParameter("@ClientLastName", SqlDbType.NVarChar, 100) { Value = item.ClientLastName });
                sqlCommand.Parameters.Add(new SqlParameter("@ClientWorkPosition", SqlDbType.NVarChar, 200) { Value = item.ClientWorkPosition });

                connection.Open();
                try
                {
                    sqlCommand.ExecuteNonQuery();
                }
                finally
                {
                }
            }
        }

        /// <summary>
        /// Add New Slider
        /// </summary>
        /// <param name="item">Object of SliderItem</param>
        /// <returns>new SliderID</returns>
        public string AddNewSlider(SliderItem item)
        {
            using (var connection = Config.SqlConnectionString)
            using (var sqlCommand = new SqlCommand("INSERT INTO rb_sliders (ModuleId,ClientWorkPosition, ClientQuote, ClientFirstName, ClientLastName) VALUES (@ModuleId, @ClientWorkPosition, @ClientQuote, @ClientFirstName, @ClientLastName)", connection))
            {
                // Mark the Command as a SPROC
                sqlCommand.CommandType = CommandType.Text;
                sqlCommand.Parameters.Add(new SqlParameter("@ModuleId", SqlDbType.Int) { Value = item.ModuleId });
                sqlCommand.Parameters.Add(new SqlParameter("@ClientQuote", SqlDbType.NVarChar, 2000) { Value = item.ClientQuote });
                sqlCommand.Parameters.Add(new SqlParameter("@ClientFirstName", SqlDbType.NVarChar, 100) { Value = item.ClientFirstName });
                sqlCommand.Parameters.Add(new SqlParameter("@ClientLastName", SqlDbType.NVarChar, 100) { Value = item.ClientLastName });
                sqlCommand.Parameters.Add(new SqlParameter("@ClientWorkPosition", SqlDbType.NVarChar, 200) { Value = item.ClientWorkPosition });

                connection.Open();
                try
                {
                    sqlCommand.ExecuteNonQuery();
                    return "";
                }
                finally
                {
                }
            }
        }

        /// <summary>
        /// Delete Slider and its respective Images also
        /// </summary>
        /// <param name="sliderid">Slider ID</param>
        public void DeleteSlider(int sliderid)
        {
            using (var connection = Config.SqlConnectionString)
            using (var sqlCommand = new SqlCommand("delete from rb_Sliders where Id = @sliderID", connection))
            {
                // Mark the Command as a SPROC
                sqlCommand.CommandType = CommandType.Text;

                var parameterSliderId = new SqlParameter("@SliderID", SqlDbType.Int, 4) { Value = sliderid };
                sqlCommand.Parameters.Add(parameterSliderId);

                connection.Open();
                try
                {
                    sqlCommand.ExecuteNonQuery();
                }
                finally
                {
                }
            }
        }


        /// <summary>
        /// Get Slider from Slider id
        /// </summary>
        /// <param name="sliderID">Slider id</param>
        /// <returns>Slider detail</returns>
        public SliderItem GetSliderByID(int sliderID)
        {
            return Appleseed.Framework.Data.DBHelper.DataReaderToObjectList<SliderItem>("select * from rb_sliders where Id =" + sliderID).First();
        }

    }
}
