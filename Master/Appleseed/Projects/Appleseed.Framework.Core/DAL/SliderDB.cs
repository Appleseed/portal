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
        #region Priavate Variables

        /// <summary>
        /// Gets or sets Output parameter
        /// </summary>
        public string Output { get; set; }

        /// <summary>
        /// Gets or sets SliderId
        /// </summary>
        public int SliderID { get; set; }

        /// <summary>
        /// Gets or sets SliderName
        /// </summary>
        public string SliderName { get; set; }

        #endregion

        /// <summary>
        /// Get all sliders from DB
        /// </summary>
        /// <returns>Slider List</returns>
        public List<SliderItem> AllSliders(int moduleId)
        {
            return Appleseed.Framework.Data.DBHelper.DataReaderToObjectList<SliderItem>("SELECT * from rb_Sliders where ModuleId = " + moduleId);
        }


        #region Slider Management

        /// <summary>
        /// Get all sliders from DB
        /// </summary>
        /// <returns>Slider List</returns>
        public List<SliderItem> AllSliders()
        {
            return Appleseed.Framework.Data.DBHelper.DataReaderToObjectList<SliderItem>("SELECT * from rb_Sliders");
        }

        /// <summary>
        /// Get all active images detail from DB by Slider id
        /// </summary>
        /// <param name="sliderId"> Slider id </param>
        /// <param name="onlyActive"> True / False </param>
        /// <returns> List of Image detail of Selected Slider </returns>
        public List<SliderImageItem> SliderImages(int sliderId, bool onlyActive = false)
        {
            if (onlyActive)
            {
                return Appleseed.Framework.Data.DBHelper.DataReaderToObjectList<SliderImageItem>("SELECT * from rb_SliderImages where Active = 1 and SliderId = " + sliderId + " order by DisplayOrder");
            }
            else
            {
                return Appleseed.Framework.Data.DBHelper.DataReaderToObjectList<SliderImageItem>("SELECT * from rb_SliderImages where SliderId = " + sliderId);
            }
        }

        /// <summary>
        /// Slider image item from selected image
        /// </summary>
        /// <param name="imageid">Slider Image ID</param>
        /// <returns>Image Detail</returns>
        public SliderImageItem SliderIamgeDetail(int imageid)
        {
            return Appleseed.Framework.Data.DBHelper.DataReaderToObjectList<SliderImageItem>("select * from rb_sliderImages where SliderImageID=" + imageid).First();
        }

        /// <summary>
        /// "Update Slider 
        /// </summary>
        /// <param name="sliderItem">object of SliderItem</param>
        public void UpdateSlider(SliderItem item)
        {
            using (var connection = Config.SqlConnectionString)
            using (var sqlCommand = new SqlCommand("update rb_sliders SET ClientWorkPosition = @ClientWorkPosition, ClientQuote = @ClientQuote, ClientFirstName = @ClientFirstName, ClientLastName = @ClientLastName, BackgroudImageUrl = @BackgroudImageUrl, BackgroudColor=@BackgroudColor  where Id = @Id", connection))
            {
                // Mark the Command as a SPROC
                sqlCommand.CommandType = CommandType.Text;

                sqlCommand.Parameters.Add(new SqlParameter("@Id", SqlDbType.Int) { Value = item.Id });
                sqlCommand.Parameters.Add(new SqlParameter("@ClientQuote", SqlDbType.NVarChar, 2000) { Value = item.ClientQuote });
                sqlCommand.Parameters.Add(new SqlParameter("@ClientFirstName", SqlDbType.NVarChar, 100) { Value = item.ClientFirstName });
                sqlCommand.Parameters.Add(new SqlParameter("@ClientLastName", SqlDbType.NVarChar, 100) { Value = item.ClientLastName });
                sqlCommand.Parameters.Add(new SqlParameter("@BackgroudImageUrl", SqlDbType.NVarChar, 1000) { Value = item.BackgroudImageUrl });
                sqlCommand.Parameters.Add(new SqlParameter("@BackgroudColor", SqlDbType.NVarChar, 100) { Value = item.BackgroudColor });
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
            using (var sqlCommand = new SqlCommand("INSERT INTO rb_sliders (ModuleId,ClientWorkPosition, ClientQuote, ClientFirstName, ClientLastName, BackgroudImageUrl, BackgroudColor) VALUES (@ModuleId, @ClientWorkPosition, @ClientQuote, @ClientFirstName, @ClientLastName, @BackgroudImageUrl, @BackgroudColor)", connection))
            {
                // Mark the Command as a SPROC
                sqlCommand.CommandType = CommandType.Text;
                sqlCommand.Parameters.Add(new SqlParameter("@ModuleId", SqlDbType.Int) { Value = item.ModuleId });
                sqlCommand.Parameters.Add(new SqlParameter("@ClientQuote", SqlDbType.NVarChar, 2000) { Value = item.ClientQuote });
                sqlCommand.Parameters.Add(new SqlParameter("@ClientFirstName", SqlDbType.NVarChar, 100) { Value = item.ClientFirstName });
                sqlCommand.Parameters.Add(new SqlParameter("@ClientLastName", SqlDbType.NVarChar, 100) { Value = item.ClientLastName });
                sqlCommand.Parameters.Add(new SqlParameter("@BackgroudImageUrl", SqlDbType.NVarChar, 1000) { Value = item.BackgroudImageUrl });
                sqlCommand.Parameters.Add(new SqlParameter("@BackgroudColor", SqlDbType.NVarChar, 100) { Value = item.BackgroudColor });
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

                var parameterOutput = new SqlParameter(this.Output, SqlDbType.Int);
                parameterOutput.Direction = ParameterDirection.Output;
                sqlCommand.Parameters.Add(parameterOutput);

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
        /// Delete Slider and its respective Images also
        /// </summary>
        /// <param name="sliderImageID"> Slider ImageID </param>
        public void DeleteSliderImage(int sliderImageID)
        {
            using (var connection = Config.SqlConnectionString)
            using (var sqlCommand = new SqlCommand("delete from rb_SliderImages where SliderImageID = @SliderImageID", connection))
            {
                // Mark the Command as a SPROC
                sqlCommand.CommandType = CommandType.Text;

                var parameterSliderImageID = new SqlParameter("@SliderImageID", SqlDbType.Int, 4) { Value = sliderImageID };
                sqlCommand.Parameters.Add(parameterSliderImageID);

                var parameterOutput = new SqlParameter(this.Output, SqlDbType.Int);
                parameterOutput.Direction = ParameterDirection.Output;
                sqlCommand.Parameters.Add(parameterOutput);

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
        /// Save/Update Image Detail
        /// </summary>
        /// <param name="sliderImage">Slider Image Object</param>
        /// <returns>New Slider Image ID</returns>
        public string SaveImageDetail(SliderImageItem sliderImage)
        {
            using (var connection = Config.SqlConnectionString)

            using (var sqlCommand = new SqlCommand("insert into rb_sliderImages (SliderID,SliderCaption,SliderImageExt,CreatedDate,CreatedUserName,Active,DisplayOrder) OUTPUT INSERTED.SliderImageID values (@SliderID,@SliderCaption,@SliderImageExt,@CreatedDate,@CreatedUserName,@Active,@DisplayOrder)", connection))
            {
                // Mark the Command as a SPROC
                sqlCommand.CommandType = CommandType.Text;

                //// Add Parameters to SPROC
                var parameterSliderID = new SqlParameter("@SliderID", SqlDbType.Int, 4) { Value = sliderImage.SliderID };
                sqlCommand.Parameters.Add(parameterSliderID);

                var parametereCreatedDate = new SqlParameter("@CreatedDate", SqlDbType.DateTime) { Value = sliderImage.CreatedDate };
                sqlCommand.Parameters.Add(parametereCreatedDate);

                var parametereCreatedUserName = new SqlParameter("@CreatedUserName", SqlDbType.VarChar, 500) { Value = sliderImage.CreatedUserName };
                sqlCommand.Parameters.Add(parametereCreatedUserName);

                Imageparameters(sliderImage, sqlCommand);

                var parameterOutput = new SqlParameter(this.Output, SqlDbType.Int);
                parameterOutput.Direction = ParameterDirection.Output;
                sqlCommand.Parameters.Add(parameterOutput);

                connection.Open();
                try
                {
                    return sqlCommand.ExecuteScalar().ToString();
                }
                finally
                {
                }
            }
        }

        /// <summary>
        /// Update Image Detail
        /// </summary>
        /// <param name="sliderImage">SliderImageItem Object</param>
        public void UpdateImageDetail(SliderImageItem sliderImage)
        {
            using (var connection = Config.SqlConnectionString)
            using (var sqlCommand = new SqlCommand("Update rb_sliderImages set SliderCaption=@SliderCaption ,SliderImageExt=@SliderImageExt, UpdatedDate=@UpdatedDate,UpdatedUserName=@UpdatedUserName,Active=@Active,DisplayOrder=@DisplayOrder where SliderImageID=@SliderImageID", connection))
            {
                // Mark the Command as a SPROC
                sqlCommand.CommandType = CommandType.Text;

                //// Add Parameters to SPROC

                var parameterSliderImageID = new SqlParameter("@SliderImageID", SqlDbType.Int, 4) { Value = sliderImage.SliderImageID };
                sqlCommand.Parameters.Add(parameterSliderImageID);

                var parametereUpdatedDate = new SqlParameter("@UpdatedDate", SqlDbType.DateTime) { Value = sliderImage.UpdatedDate };
                sqlCommand.Parameters.Add(parametereUpdatedDate);

                var parametereUpdatedUserName = new SqlParameter("@UpdatedUserName", SqlDbType.VarChar, 500) { Value = sliderImage.UpdatedUserName };
                sqlCommand.Parameters.Add(parametereUpdatedUserName);

                Imageparameters(sliderImage, sqlCommand);

                var parameterOutput = new SqlParameter(this.Output, SqlDbType.Int);
                parameterOutput.Direction = ParameterDirection.Output;
                sqlCommand.Parameters.Add(parameterOutput);

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
        /// Get Slider detail by slider name
        /// </summary>
        /// <param name="sliderName"> Slider name </param>
        /// <returns> 0/1 </returns>
        public int GetSliderByName(string sliderName)
        {
            using (var connection = Config.SqlConnectionString)
            using (var sqlCommand = new SqlCommand("select count(sliderName) from rb_sliders where SliderName=@sliderName", connection))
            {
                // Mark the Command as a SPROC
                sqlCommand.CommandType = CommandType.Text;
                var parameterSliderName = new SqlParameter("@sliderName", SqlDbType.NVarChar, 200) { Value = sliderName };
                sqlCommand.Parameters.Add(parameterSliderName);

                var parameterOutput = new SqlParameter(this.Output, SqlDbType.Int);
                parameterOutput.Direction = ParameterDirection.Output;
                sqlCommand.Parameters.Add(parameterOutput);

                connection.Open();
                try
                {
                    return Convert.ToInt32(sqlCommand.ExecuteScalar());
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

        /// <summary>
        /// Get Max display order
        /// </summary>
        /// <param name="sliderID"> SliderID </param>
        /// <returns >Maximum display order </returns>
        public int GetMaxDisplayOrder(int sliderID)
        {
            using (var connection = Config.SqlConnectionString)
            using (var sqlCommand = new SqlCommand("select max(displayorder)+1 from rb_SliderImages where SliderID=@SliderID", connection))
            {
                // Mark the Command as a SPROC
                sqlCommand.CommandType = CommandType.Text;
                var parameterSliderName = new SqlParameter("@sliderID", SqlDbType.Int) { Value = sliderID };
                sqlCommand.Parameters.Add(parameterSliderName);

                var parameterOutput = new SqlParameter(this.Output, SqlDbType.Int);
                parameterOutput.Direction = ParameterDirection.Output;
                sqlCommand.Parameters.Add(parameterOutput);

                connection.Open();
                try
                {
                    return Convert.ToInt32(sqlCommand.ExecuteScalar());
                }
                finally
                {
                }
            }
        }

        /// <summary>
        /// Common image parameters
        /// </summary>
        /// <param name="sliderImage"> slider image</param>
        /// <param name="sqlCommand"> parameter</param>
        private static void Imageparameters(SliderImageItem sliderImage, SqlCommand sqlCommand)
        {
            var parametereSliderCaption = new SqlParameter("@SliderCaption", SqlDbType.NVarChar, 200) { Value = sliderImage.SliderCaption };
            sqlCommand.Parameters.Add(parametereSliderCaption);

            var parametereSliderImageExt = new SqlParameter("@SliderImageExt", SqlDbType.NVarChar, 5) { Value = sliderImage.SliderImageExt };
            sqlCommand.Parameters.Add(parametereSliderImageExt);

            var parametereActive = new SqlParameter("@Active", SqlDbType.Bit, 0) { Value = sliderImage.Active };
            sqlCommand.Parameters.Add(parametereActive);

            var parametereDisplayOrder = new SqlParameter("@DisplayOrder", SqlDbType.Int, 4) { Value = sliderImage.DisplayOrder };
            sqlCommand.Parameters.Add(parametereDisplayOrder);
        }
        #endregion
    }
}
