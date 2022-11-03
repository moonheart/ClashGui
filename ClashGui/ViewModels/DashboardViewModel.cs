﻿using System;
using System.Reactive;
using System.Threading.Tasks;
using ClashGui.Cli;
using ClashGui.Cli.ClashConfigs;
using ClashGui.Interfaces;
using ClashGui.Utils;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;

namespace ClashGui.ViewModels;

public class DashboardViewModel : ViewModelBase, IDashboardViewModel
{
    private IClashCli _clashCli;

    public DashboardViewModel(IClashCli clashCli)
    {
        _clashCli = clashCli;

        StartClash = ReactiveCommand.CreateFromTask(async () =>
        {
            var rawConfig = await _clashCli.Start();
            ProxyUtils.SetSystemProxy($"http://127.0.0.1:{rawConfig.MixedPort ?? rawConfig.Port}", "");
            _rawConfig = rawConfig;
        });

        StopClash = ReactiveCommand.CreateFromTask(async () =>
        {
            await _clashCli.Stop();
            ProxyUtils.UnsetSystemProxy();
        });

        _clashCli.RunningObservable.BindTo(this, d => d.RunningState);

        _clashCli.RunningObservable.Subscribe(d =>
        {
            IsStartingOrStopping = d == RunningState.Starting || d == RunningState.Stopping;
            IsStarted = d == RunningState.Started;
            IsStopped = d == RunningState.Stopped;
        });
    }

    [Reactive]
    private RawConfig _rawConfig { get; set; }

    public override string Name => "Dashboard";
    public ReactiveCommand<Unit, Unit> StartClash { get; }
    public ReactiveCommand<Unit, Unit> StopClash { get; }

    [Reactive]
    public RunningState RunningState { get; set; }

    [Reactive]
    public bool IsStartingOrStopping { get; set; }

    [Reactive]
    public bool IsStarted { get; set; }

    [Reactive]
    public bool IsStopped { get; set; }
}