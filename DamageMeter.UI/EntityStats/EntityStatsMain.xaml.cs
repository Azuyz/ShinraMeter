﻿using System.Collections.Generic;
using System.Windows;
using System.Windows.Interop;
using DamageMeter.Database.Structures;
using Data;
using Tera.Game.Abnormality;

namespace DamageMeter.UI.EntityStats
{
    /// <summary>
    ///     Logique d'interaction pour EntityStats.xaml
    /// </summary>
    public partial class EntityStatsMain
    {
        private readonly List<EnduranceDebuff> _enduranceDebuffsList = new List<EnduranceDebuff>();

        private readonly EnduranceDebuffHeader _header;
        private readonly MainWindow _parent;

        public EntityStatsMain(MainWindow parent)
        {
            InitializeComponent();
            _parent = parent;
            CloseWindow.Source = BasicTeraData.Instance.ImageDatabase.Close.Source;
            _header = new EnduranceDebuffHeader();
        }

        public void SetClickThrou()
        {
            var hwnd = new WindowInteropHelper(this).Handle;
            WindowsServices.SetWindowExTransparent(hwnd);
        }

        public void UnsetClickThrou()
        {
            var hwnd = new WindowInteropHelper(this).Handle;
            WindowsServices.SetWindowExVisible(hwnd);
        }

        public void Update(EntityInformation entityInformation, AbnormalityStorage abnormals)
        {
            EnduranceAbnormality.Items.Clear();
            if (entityInformation == null)
            {
                return;
            }
            if (entityInformation.Interval == 0)
            {
                return;
            }

            EnduranceAbnormality.Items.Add(_header);

            var count = 0;
            foreach (var abnormality in abnormals.Get(entityInformation.Entity))
            {
                EnduranceDebuff abnormalityUi;
                if (_enduranceDebuffsList.Count > count)
                {
                    abnormalityUi = _enduranceDebuffsList[count];
                }
                else
                {
                    abnormalityUi = new EnduranceDebuff();
                    _enduranceDebuffsList.Add(abnormalityUi);
                }

                abnormalityUi.Update(abnormality.Key, abnormality.Value,
                    entityInformation.BeginTime,
                    entityInformation.EndTime);
                EnduranceAbnormality.Items.Add(abnormalityUi);
                count++;
            }
        }

        private void CloseMeter_OnClick(object sender, RoutedEventArgs e)
        {
            _parent.CloseEntityStats();
        }
    }
}