using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace AppCore.UseCases.DT.Models
{
    public class DTCosing : IDTCosting
    {
        [JsonPropertyName("Id")]
        public string ProductId { get; set; }

        public string SupplierId { get; set; }
        public string? Status { get; set; }
        public string? Remark { get; set; }

        public DateTime? ValidFrom { get; set; }
        public DateTime? ValidTo { get; set; }

        public string? AnnexLink { get; set; }

        public string? ModeOfTransportCode { get; set; }
        public string? ModeOfTransportNameEN { get; set; }
        public string? ModeOfTransportNameVN { get; set; }

        public string? UoMOfTransportCode { get; set; }
        public string? UoMOfTransportNameEN { get; set; }
        public string? UoMOfTransportNameVN { get; set; }

        public string? ChargeCode { get; set; }
        public string? ChargeNameEN { get; set; }
        public string? ChargeNameVN { get; set; }

        public string? AdditionalRequest { get; set; }

        public string? RailServiceType { get; set; }

        public string? AirlineCarrierIATA { get; set; }
        public string? AirlineCarrierName { get; set; }

        public string? OriginProvinceCode { get; set; }
        public string? OriginProvinceNameEN { get; set; }
        public string? OriginProvinceNameVN { get; set; }

        public int OriginDistrictId { get; set; }
        public string? OriginDistrictNameEN { get; set; }
        public string? OriginDistrictNameVN { get; set; }

        public string? DestinationProvinceCode { get; set; }
        public string? DestinationProvinceNameEN { get; set; }
        public string? DestinationProvinceNameVN { get; set; }

        public int DestinationDistrictId { get; set; }
        public string? DestinationDistrictNameEN { get; set; }
        public string? DestinationDistrictNameVN { get; set; }

        /// <summary>
        ///     Air,Sea,Rail
        /// </summary>
        public string? OriginPortType { get; set; }

        public int OriginPortId { get; set; }
        public string? OriginPortCode { get; set; }
        public string? OriginPortNameEN { get; set; }
        public string? OriginPortNameVN { get; set; }

        public string? DestinationPortType { get; set; }
        public int DestinationPortId { get; set; }
        public string? DestinationPortCode { get; set; }
        public string? DestinationPortNameEN { get; set; }
        public string? DestinationPortNameVN { get; set; }

        public string? VolumeRangePerMonthUoM { get; set; }
        public string? VolumeRangePerMonthName { get; set; }
        public string? VolumeRangePerUoMUoM { get; set; }
        public string? VolumeRangePerUoMName { get; set; }

        public decimal Cost { get; set; }
        public string? Currency { get; set; }
        public string? LeadTime { get; set; }

        [JsonIgnore]
        public string[] CapacityFields { get; set; } =
        {
            "ModeOfTransportCode", "UoMOfTransportCode"
        };

        public string GetCapacityKey()
        {
            StringBuilder stringBuilder = new();

            Type type = GetType();
            foreach (string fieldName in CapacityFields)
            {
                PropertyInfo? prop = type.GetProperty(fieldName);
                stringBuilder.Append($"[{prop?.GetValue(this)}]");
            }

            return stringBuilder.ToString();
        }

        [JsonIgnore]
        public string?[] ProductFields { get; set; } =
   {
        "ModeOfTransportCode", "RailServiceType", "AirlineCarrierIATA", "AdditionalRequest", "OriginDistrictId",
        "OriginProvinceCode", "OriginPortCode", "DestinationDistrictId", "DestinationProvinceCode",
        "DestinationPortCode", "VolumeRangePerMonthName", "VolumeRangePerMonthUoM", "VolumeRangePerUoMName",
        "VolumeRangePerUoMUoM", "UoMOfTransportCode", "LeadTime"
    };

        /// <summary>
        ///     This field use for Map Dto ProductKey props
        /// </summary>
        /// <returns></returns>
        public string GetProductKey()
        {
            StringBuilder stringBuilder = new();

            Type type = GetType();
            foreach (string fieldName in ProductFields)
            {
                PropertyInfo? prop = type.GetProperty(fieldName);
                stringBuilder.Append($"[{prop?.GetValue(this)}]");
            }

            return stringBuilder.ToString();
        }
    }
}
