namespace Appleseed.Framework.Configuration.Items
{
    using System;

    /// <summary>
    /// SliderItem
    /// </summary>
    public class SliderItem
    {
        /// <summary>
        /// Slider ID
        /// </summary>
        public int SliderID { get; set; }

        /// <summary>
        /// Slider Name
        /// </summary>
        public string SliderName { get; set; }

        /// <summary>
        /// Slider Created Date
        /// </summary>
        public DateTime CreatedDate { get; set; }

        /// <summary>
        /// Slider Created by UserName
        /// </summary>
        public string CreatedUserName { get; set; }

        /// <summary>
        /// Slider Updatedate
        /// </summary>
        public DateTime UpdateDate { get; set; }

        /// <summary>
        /// Slider Update by UserName
        /// </summary>
        public string UpdatedUserName { get; set; }

        public int Id { get; set; }
        public int ModuleId { get; set; }
        public string ClientQuote { get; set; }
        public string ClientFirstName { get; set; }
        public string ClientLastName { get; set; }
        public string BackgroudImageUrl { get; set; }
        public string BackgroudColor { get; set; }
        public string ClientWorkPosition { get; set; }
    }
}
