﻿using System.Collections.Generic;
using System.Linq;
using DamageMeter.Database.Structures;
using DamageMeter.UI.EntityStats;
using Tera.Game.Abnormality;

namespace DamageMeter.UI
{
    /// <summary>
    ///     Logique d'interaction pour Buff.xaml
    /// </summary>
    public partial class Buff
    {
        private readonly List<EnduranceDebuff> _enduranceDebuffsList = new List<EnduranceDebuff>();

        private readonly EnduranceDebuffHeader _header;

        public Buff(PlayerDamageDealt playerDamageDealt, PlayerAbnormals buffs, EntityInformation entityInformation)
        {
            InitializeComponent();
            _header = new EnduranceDebuffHeader();
            ContentWidth = 1020;

            EnduranceAbnormality.Items.Clear();
            EnduranceAbnormality.Items.Add(_header);
            var counter = 0;
            foreach (var abnormality in buffs.Times.Where(x => x.Value.Duration(playerDamageDealt.BeginTime, playerDamageDealt.EndTime) > 0))
            {
                EnduranceDebuff abnormalityUi;
                if (_enduranceDebuffsList.Count > counter)
                {
                    abnormalityUi = _enduranceDebuffsList[counter];
                }
                else
                {
                    abnormalityUi = new EnduranceDebuff();
                    _enduranceDebuffsList.Add(abnormalityUi);
                }
                abnormalityUi.Update(abnormality.Key, abnormality.Value, playerDamageDealt.BeginTime, playerDamageDealt.EndTime);
                EnduranceAbnormality.Items.Add(abnormalityUi);

                counter++;
            }
        }

        public double ContentWidth { get; private set; }

        private void EnduranceAbnormality_PreviewMouseWheel(object sender, System.Windows.Input.MouseWheelEventArgs e)
        {
            ScrollViewer.ScrollToVerticalOffset(ScrollViewer.VerticalOffset - e.Delta);
            e.Handled = true;
        }
    }
}