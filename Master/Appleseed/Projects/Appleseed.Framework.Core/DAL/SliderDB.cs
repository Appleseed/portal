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
        public void UpdateSlider(SliderItem sliderItem)
        {
            using (var connection = Config.SqlConnectionString)
            using (var sqlCommand = new SqlCommand("update rb_sliders SET SliderName = @sliderName, UpdatedDate = @UpdatedDate, UpdatedUserName = @UpdatedUserName  where SliderID = @sliderId", connection))
            {
                // Mark the Command as a SPROC
                sqlCommand.CommandType = CommandType.Text;

                var parameterSliderId = new SqlParameter("@SliderID", SqlDbType.Int, 4) { Value = sliderItem.SliderID };
                sqlCommand.Parameters.Add(parameterSliderId);

                var parameterSliderName = new SqlParameter("@SliderName", SqlDbType.NVarChar, 200) { Value = sliderItem.SliderName };
                sqlCommand.Parameters.Add(parameterSliderName);

                var parameterUpdatedDate = new SqlParameter("@UpdatedDate", SqlDbType.DateTime) { Value = sliderItem.UpdateDate };
                sqlCommand.Parameters.Add(parameterUpdatedDate);

                var parameterUpdatedUserName = new SqlParameter("@UpdatedUserName", SqlDbType.NVarChar, 500) { Value = sliderItem.UpdatedUserName };
                sqlCommand.Parameters.Add(parameterUpdatedUserName);

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
        /// Add New Slider
        /// </summary>
        /// <param name="item">Object of SliderItem</param>
        /// <returns>new SliderID</returns>
        public string AddNewSlider(SliderItem item)
        {
            using (var connection = Config.SqlConnectionString)
            using (var sqlCommand = new SqlCommand("INSERT INTO rb_sliders (SliderName) OUTPUT INSERTED.SliderID VALUES (@sliderName)", connection))
            {
                // Mark the Command as a SPROC
                sqlCommand.CommandType = CommandType.Text;
                var parameterSliderName = new SqlParameter("@SliderName", SqlDbType.NVarChar, 200) { Value = item.SliderName };
                sqlCommand.Parameters.Add(parameterSliderName);

                sqlCommand.CommandType = CommandType.Text;
                var parameterCreatedDate = new SqlParameter("@CreatedDate", SqlDbType.NVarChar, 200) { Value = item.CreatedDate };
                sqlCommand.Parameters.Add(parameterCreatedDate);

                sqlCommand.CommandType = CommandType.Text;
                var parameterCreatedUserName = new SqlParameter("@CreatedUserName", SqlDbType.NVarChar, 200) { Value = item.CreatedUserName };
                sqlCommand.Parameters.Add(parameterCreatedUserName);

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
        /// Delete Slider and its respective Images also
        /// </summary>
        /// <param name="sliderid">Slider ID</param>
        public void DeleteSlider(int sliderid)
        {
            using (var connection = Config.SqlConnectionString)
            using (var sqlCommand = new SqlCommand("delete from rb_Sliders where SliderID = @sliderID", connection))
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
            return Appleseed.Framework.Data.DBHelper.DataReaderToObjectList<SliderItem>("select * from rb_sliders where sliderid=" + sliderID).First();
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
