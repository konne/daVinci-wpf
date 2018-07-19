﻿namespace daVinci.ConfigData.Loop
{
    #region Usings
    using Hjson;
    using leonardo.Resources;
    using Newtonsoft.Json.Linq;
    using NLog;
    using System;
    using System.ComponentModel;
    using System.Runtime.CompilerServices;
    #endregion

    public class LoopConfiguration : INotifyPropertyChanged
    {
        #region LoggerInit
        private static Logger logger = LogManager.GetCurrentClassLogger();
        #endregion

        #region INotifyPropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;
        private void RaisePropertyChanged([CallerMemberName] string caller = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(caller));
            FillValues(caller);
        }
        #endregion

        #region Properties & Variables
        private bool exportRootNode = true;
        public bool ExportRootNode
        {
            get => exportRootNode;
            set
            {
                if (exportRootNode != value)
                {
                    exportRootNode = value;
                    RaisePropertyChanged();
                }
            }
        }
        private string sheetNameExpression;
        public string SheetNameExpression
        {
            get => sheetNameExpression;
            set
            {
                if (sheetNameExpression != value)
                {
                    sheetNameExpression = value;
                    RaisePropertyChanged();
                }
            }
        }
        private string expressionText;
        public string ExpressionText
        {
            get => expressionText;
            set
            {
                if (expressionText != value)
                {
                    expressionText = value;
                    RaisePropertyChanged();
                }
            }
        }
        private bool enableExpression;
        public bool EnableExpression
        {
            get => enableExpression;
            set
            {
                if (enableExpression != value)
                {
                    enableExpression = value;
                    RaisePropertyChanged();
                }
            }
        }
        private DimensionMeasure loopOver;
        public DimensionMeasure LoopOver
        {
            get => loopOver;
            set
            {
                if (loopOver != value)
                {
                    loopOver = value;
                    RaisePropertyChanged();
                }
            }
        }
        private bool dontProcessFill;
        #endregion

        #region private Functions

        private void FillValues(string propertyName)
        {
            if (dontProcessFill)
                return;

            if (propertyName != nameof(ExpressionText))
            {
                dynamic data = null;
                try
                {
                    if (string.IsNullOrEmpty(ExpressionText))
                    {
                        ExpressionText = $"selections:\n[\n  {{\n    type: dynamic\n  \n  }}\n]";
                    }
                    var value = HjsonValue.Parse(ExpressionText);
                    data = JObject.Parse(value.ToString());
                }
                catch (Exception ex)
                {
                    logger.Error(ex, "Error Parsing Json");
                }
                try
                {
                    if (data != null)
                    {
                        if ((data?.selections?.Count ?? 0) > 0)
                        {
                            if (propertyName == nameof(LoopOver))
                                data.selections[0].name = LoopOver.Text;
                            if (propertyName == nameof(ExportRootNode))
                                data.selections[0].exportRootNode = ExportRootNode;
                            if (propertyName == nameof(SheetNameExpression))
                                data.selections[0].sheetName = SheetNameExpression;
                        }
                        var text = HjsonValue.Parse(data.ToString()).ToString(Stringify.Hjson);
                        dontProcessFill = true;
                        ExpressionText = text.Substring(1, text.Length - 2).Trim();
                        dontProcessFill = false;
                    }
                }
                catch (Exception ex)
                {
                    logger.Error(ex, "Error Setting Values");
                    dontProcessFill = false;
                }

            }
            else
            {
                dynamic data = null;
                try
                {
                    if (string.IsNullOrEmpty(ExpressionText))
                    {
                        dontProcessFill = true;
                        ExpressionText = $"selections:\n[\n  {{\n    type: dynamic\n  \n  }}\n]";
                        dontProcessFill = false;
                    }
                    var value = HjsonValue.Parse(ExpressionText);
                    data = JObject.Parse(value.ToString());
                    if (data != null)
                    {
                        if ((data?.selections?.Count ?? 0) > 0)
                        {
                            dontProcessFill = true;
                            ExportRootNode = (data.selections[0]?.exportRootNode?.ToString() ?? "true") != "False";
                            SheetNameExpression = data.selections[0]?.sheetName ?? "";
                            dontProcessFill = false;
                        }
                    }
                }
                catch (Exception ex)
                {
                    logger.Error(ex, $"Error Processing Loopconfiguration.{nameof(ExpressionText)}");
                    dontProcessFill = false;
                }
                if (data != null)
                {

                }
            }
        }
        #endregion
    }
}
