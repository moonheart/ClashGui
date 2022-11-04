﻿using System.Collections.ObjectModel;
using System.Reactive;
using ClashGui.Interfaces;
using ClashGui.ViewModels;
using DynamicData;
using ReactiveUI;

namespace ClashGui.DesignTime
{
    public class DesignMainWindowViewModel : ViewModelBase, IMainWindowViewModel
    {
        public DesignMainWindowViewModel()
        {
            ProxiesViewModel = new DesignProxiesViewModel();
            ClashLogsViewModel = new DesignClashLogsViewModel();
            ProxyRulesListViewModel = new DesignProxyRulesListViewModel();
            ConnectionsViewModel = new DesignConnectionsViewModel();
            ClashInfoViewModel = new DesignClashInfoViewModel();

            Selections = new ObservableCollection<IViewModelBase>();
            Selections.AddRange(new IViewModelBase[]
            {
                ProxiesViewModel,
                ClashLogsViewModel,
                ProxyRulesListViewModel,
                ConnectionsViewModel,
            });
            CurrentViewModel = ProxiesViewModel;
        }

        public IViewModelBase CurrentViewModel { get; set; }

        public IProxiesViewModel ProxiesViewModel { get; }

        public IClashLogsViewModel ClashLogsViewModel { get; }

        public IProxyRulesListViewModel ProxyRulesListViewModel { get; }

        public IConnectionsViewModel ConnectionsViewModel { get; }
        public IClashInfoViewModel ClashInfoViewModel { get; }
        public ISettingsViewModel SettingsViewModel { get; }
        public ObservableCollection<IViewModelBase> Selections { get; }
    }
}