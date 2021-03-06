﻿namespace daVinci.ConfigData
{
    #region Usings
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Linq;
    using System.Runtime.CompilerServices;
    using daVinci.ConfigData.TableConfigurations;
    using leonardo.Resources;
    using Newtonsoft.Json.Linq;
    using NLog;
    #endregion

    public class MeasureColumnData : ColumnData, IHasSortCriteria
    {
        #region Logger
        private static Logger logger = LogManager.GetCurrentClassLogger();
        #endregion

        #region Properties & Variables
        private int numberFormatIndex;
        public int NumberFormatIndex
        {
            get
            {
                return numberFormatIndex;
            }
            set
            {
                if (numberFormatIndex != value)
                {
                    numberFormatIndex = value;

                    switch (numberFormatIndex)
                    {
                        case 1:
                            NumberFormatText = "#,##0.00";
                            break;
                        case 2:
                            CurrencyFormatText = "$#,##0.00;($#,##0.00)";
                            break;
                        case 3:
                            DateFormatText = "M/D/YYYY";
                            break;
                        case 4:
                            DurationFormatText = "h:mm:ss";
                            break;
                        case 5:
                            Thou_SplitterSign = ",";
                            Dec_SplitterSign = ".";
                            break;
                        default:
                            break;
                    }

                    RaisePropertyChanged();
                }
            }
        }

        #region Number
        private bool isStandardFormat;
        public bool IsStandardFormat
        {
            get
            {
                return isStandardFormat;
            }
            set
            {
                if (isStandardFormat != value)
                {
                    isStandardFormat = value;
                    RaisePropertyChanged();
                }
            }
        }

        private int standardFormatIndex;
        public int StandardFormatIndex
        {
            get
            {
                return standardFormatIndex;
            }
            set
            {
                if (standardFormatIndex != value)
                {
                    standardFormatIndex = value;
                    RaisePropertyChanged();
                }
            }
        }

        private string numberFormatText;
        public string NumberFormatText
        {
            get
            {
                return numberFormatText;
            }
            set
            {
                if (numberFormatText != value)
                {
                    numberFormatText = value;
                    RaisePropertyChanged();
                }
            }
        }
        #endregion

        #region Currency
        private string currencyFormatText;
        public string CurrencyFormatText
        {
            get
            {
                return currencyFormatText;
            }
            set
            {
                if (currencyFormatText != value)
                {
                    currencyFormatText = value;
                    RaisePropertyChanged();
                }
            }
        }
        #endregion

        #region Date
        private bool isStandardDateFormat;
        public bool IsStandardDateFormat
        {
            get
            {
                return isStandardDateFormat;
            }
            set
            {
                if (isStandardDateFormat != value)
                {
                    isStandardDateFormat = value;
                    RaisePropertyChanged();
                }
            }
        }

        private int dateStandardFormatIndex;
        public int DateStandardFormatIndex
        {
            get
            {
                return dateStandardFormatIndex;
            }
            set
            {
                if (dateStandardFormatIndex != value)
                {
                    dateStandardFormatIndex = value;
                    if (DateToIndex.ContainsKey(value))
                    {
                        DateFormatText = DateToIndex[value];
                    }
                    RaisePropertyChanged();
                }
            }
        }

        private string dateFormatText;
        public string DateFormatText
        {
            get
            {
                return dateFormatText;
            }
            set
            {
                if (dateFormatText != value)
                {
                    dateFormatText = value;
                    RaisePropertyChanged();
                }
            }
        }
        #endregion

        #region Duration
        private string durationFormatText;
        public string DurationFormatText
        {
            get
            {
                return durationFormatText;
            }
            set
            {
                if (durationFormatText != value)
                {
                    durationFormatText = value;
                    RaisePropertyChanged();
                }
            }
        }
        #endregion

        #region Custom
        private string customNumberFormatText;
        public string CustomNumberFormatText
        {
            get
            {
                return customNumberFormatText;
            }
            set
            {
                if (customNumberFormatText != value)
                {
                    customNumberFormatText = value;
                    RaisePropertyChanged();
                }
            }
        }

        private string dec_SplitterSign;
        public string Dec_SplitterSign
        {
            get
            {
                return dec_SplitterSign;
            }
            set
            {
                if (dec_SplitterSign != value)
                {
                    dec_SplitterSign = value;
                    RaisePropertyChanged();
                }
            }
        }

        private string thou_SplitterSign;
        public string Thou_SplitterSign
        {
            get
            {
                return thou_SplitterSign;
            }
            set
            {
                if (thou_SplitterSign != value)
                {
                    thou_SplitterSign = value;
                    RaisePropertyChanged();
                }
            }
        }
        #endregion

        private int totalValueFunctionIndex;
        public int TotalValueFunctionIndex
        {
            get
            {
                return totalValueFunctionIndex;
            }
            set
            {
                if (totalValueFunctionIndex != value)
                {
                    totalValueFunctionIndex = value;
                    RaisePropertyChanged();
                }
            }
        }

        private bool isTotalValueSettingsTextVisible;
        public bool IsTotalValueSettingsTextVisible
        {
            get
            {
                return isTotalValueSettingsTextVisible;
            }
            set
            {
                if (isTotalValueSettingsTextVisible != value)
                {
                    isTotalValueSettingsTextVisible = value;
                    RaisePropertyChanged();
                }
            }
        }

        #endregion
        public void ReadFromJSON(dynamic jsonConfig)
        {
            try
            {
                ReadBaseDataFromJSON(jsonConfig);

                FieldDef = jsonConfig?.qDef?.qDef ?? "";
                FieldLabel = jsonConfig?.qDef?.qLabel ?? "";

                SortCriterias.ReadFromJSON(jsonConfig?.qSortBy);
                SortCriterias.AutoSort = jsonConfig.qDef?.autoSort ?? false;

                switch (jsonConfig?.qDef?.qNumFormat?.qType?.ToString() ?? "U")
                {
                    case "U":
                        NumberFormatIndex = 0;
                        break;
                    case "F":
                        NumberFormatIndex = 1;
                        IsStandardFormat = jsonConfig?.qDef?.numFormatFromTemplate ?? false;
                        string value = jsonConfig?.qDef?.qNumFormat?.qFmt?.ToString() ?? "#,##0.0";
                        switch (value?.ToString())
                        {
                            case "0.00%":
                                StandardFormatIndex = 5;
                                break;
                            case "0.0%":
                                StandardFormatIndex = 4;
                                break;
                            case "0%":
                                StandardFormatIndex = 3;
                                break;
                            case "#,##0.00":
                                StandardFormatIndex = 2;
                                break;
                            case "#,##0.0":
                                StandardFormatIndex = 1;
                                break;
                            case "#,##0":
                                StandardFormatIndex = 0;
                                break;
                            default:
                                break;
                        }
                        NumberFormatText = jsonConfig?.qDef?.qNumFormat?.qFmt ?? "";
                        break;
                    case "M":
                        NumberFormatIndex = 2;
                        CurrencyFormatText = jsonConfig?.qDef?.qNumFormat?.qFmt ?? "";
                        break;
                    case "D":
                        IsStandardDateFormat = jsonConfig?.qDef?.numFormatFromTemplate ?? false;
                        DateFormatText = jsonConfig?.qDef?.qNumFormat?.qFmt ?? "";
                        NumberFormatIndex = 3;
                        value = jsonConfig?.qDef?.qNumFormat?.qFmt?.ToString() ?? "#,##0.0";
                        DateStandardFormatIndex = DateToIndex.Where(ele => ele.Value == value).DefaultIfEmpty(new KeyValuePair<int, string>(0, "notUsed")).Single().Key;
                        break;
                    case "IV":
                        DurationFormatText = jsonConfig?.qDef?.qNumFormat?.qFmt ?? "";
                        NumberFormatIndex = 4;
                        break;
                    case "R":
                        CustomNumberFormatText = jsonConfig?.qDef?.qNumFormat?.qFmt ?? "";
                        Dec_SplitterSign = jsonConfig?.qDef?.qNumFormat?.qDec ?? ".";
                        Thou_SplitterSign = jsonConfig?.qDef?.qNumFormat?.qThou ?? ",";
                        NumberFormatIndex = 5;
                        break;
                    default:
                        break;
                }
                Dec_SplitterSign = jsonConfig?.qDef?.qNumFormat?.qDec ?? ".";
                Thou_SplitterSign = jsonConfig?.qDef?.qNumFormat?.qThou ?? ",";

                switch (jsonConfig?.qDef?.qAggrFunc?.ToString() ?? "Expr")
                {
                    case "Expr":
                        TotalValueFunctionIndex = 0;
                        break;
                    case "Avg":
                        TotalValueFunctionIndex = 1;
                        break;
                    case "Count":
                        TotalValueFunctionIndex = 2;
                        break;
                    case "Max":
                        TotalValueFunctionIndex = 3;
                        break;
                    case "Min":
                        TotalValueFunctionIndex = 4;
                        break;
                    case "Sum":
                        TotalValueFunctionIndex = 5;
                        break;
                    case "None":
                        TotalValueFunctionIndex = 6;
                        break;
                    default:
                        break;
                }

                IsTotalValueSettingsTextVisible = true;
            }
            catch (Exception Ex)
            {
                logger.Error(Ex);
                logger.Trace($"JSON:{jsonConfig?.ToString() ?? ""}");
            }
        }

        public dynamic SaveToJson()
        {
            dynamic jsonConfig = null;

            try
            {
                jsonConfig = SaveBaseDataToJson();
                jsonConfig.qDef.qLabel = FieldLabel;
                jsonConfig.qDef.qDef = FieldDef;

                jsonConfig.qSortBy = SortCriterias.SaveToJSON();
                if (SortCriterias.AutoSort)
                    jsonConfig.qDef.autoSort = SortCriterias.AutoSort;

                switch (TotalValueFunctionIndex)
                {
                    case 0:
                        jsonConfig.qDef.qAggrFunc = "Expr";
                        break;
                    case 1:
                        jsonConfig.qDef.qAggrFunc = "Avg";
                        break;
                    case 2:
                        jsonConfig.qDef.qAggrFunc = "Count";
                        break;
                    case 3:
                        jsonConfig.qDef.qAggrFunc = "Max";
                        break;
                    case 4:
                        jsonConfig.qDef.qAggrFunc = "Min";
                        break;
                    case 5:
                        jsonConfig.qDef.qAggrFunc = "Sum";
                        break;
                    case 6:
                        jsonConfig.qDef.qAggrFunc = "None";
                        break;
                    default:
                        break;
                }

                jsonConfig.qDef.qNumFormat = new JObject();
                switch (NumberFormatIndex)
                {
                    case 0:
                        jsonConfig.qDef.qNumFormat.qType = "U";
                        break;
                    case 1:
                        jsonConfig.qDef.qNumFormat.qType = "F";
                        if (IsStandardFormat)
                        {
                            switch (StandardFormatIndex)
                            {
                                case 0:
                                    jsonConfig.qDef.qNumFormat.qFmt = "#,##0";
                                    break;
                                case 1:
                                    jsonConfig.qDef.qNumFormat.qFmt = "#,##0.0";
                                    break;
                                case 2:
                                    jsonConfig.qDef.qNumFormat.qFmt = "#,##0.00";
                                    break;
                                case 3:
                                    jsonConfig.qDef.qNumFormat.qFmt = "0%";
                                    break;
                                case 4:
                                    jsonConfig.qDef.qNumFormat.qFmt = "0.0%";
                                    break;
                                case 5:
                                    jsonConfig.qDef.qNumFormat.qFmt = "0.00%";
                                    break;
                                default:
                                    break;
                            }
                        }
                        else
                        {
                            jsonConfig.qDef.qNumFormat.qFmt = NumberFormatText;
                        }
                        if (IsStandardFormat)
                            jsonConfig.qDef.numFormatFromTemplate = IsStandardFormat;
                        break;
                    case 2:
                        jsonConfig.qDef.qNumFormat.qType = "M";
                        jsonConfig.qDef.qNumFormat.qFmt = CurrencyFormatText;
                        break;
                    case 3:
                        jsonConfig.qDef.qNumFormat.qType = "D";
                        if (IsStandardDateFormat)
                        {
                            if (DateToIndex.ContainsKey(DateStandardFormatIndex))
                            {
                                jsonConfig.qDef.qNumFormat.qFmt = DateToIndex[DateStandardFormatIndex];
                            }
                            else
                            {
                                jsonConfig.qDef.qNumFormat.qFmt = "";
                            }
                        }
                        else
                        {
                            jsonConfig.qDef.qNumFormat.qFmt = DateFormatText;
                        }
                        if (IsStandardFormat)
                            jsonConfig.qDef.numFormatFromTemplate = IsStandardDateFormat;
                        break;
                    case 4:
                        jsonConfig.qDef.qNumFormat.qType = "IV";
                        jsonConfig.qDef.qNumFormat.qFmt = DurationFormatText;
                        break;
                    case 5:
                        jsonConfig.qDef.qNumFormat.qType = "R";
                        jsonConfig.qDef.qNumFormat.qFmt = CustomNumberFormatText;
                        jsonConfig.qDef.qNumFormat.qDec = Dec_SplitterSign;
                        jsonConfig.qDef.qNumFormat.qThou = Thou_SplitterSign;
                        break;
                    default:
                        break;
                }
                jsonConfig.qDef.qNumFormat.qDec = Dec_SplitterSign;
                jsonConfig.qDef.qNumFormat.qThou = Thou_SplitterSign;
            }
            catch (Exception Ex)
            {
                logger.Error(Ex);
            }
            return jsonConfig;
        }

        private Dictionary<int, string> DateToIndex = new Dictionary<int, string>()
        {
            { 0,"M/D/YYYY" },
            { 1,"YYYY-MM-DD" },
            { 2,"DD MM YYYY" },
            { 3,"DD MMMM YYYY" },
            { 4,"M/DD/YYYY" },
            { 5,"MM/DD/YYYY" },
            { 6,"MMMM DD, YYYY" },
            { 7,"M/D/YYYY h:mm:ss[.fff] TT" },
            { 8,"YYYY-MM-DD hh:mm:ss[.fff]" },
            { 9,"h:mm:ss TT" },
            { 10,"hh:mm:ss" },
            { 11,"h:mm:ss tt" }
        };
    }
}
