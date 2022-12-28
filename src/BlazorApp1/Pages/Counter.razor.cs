using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using System.Net.Http;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Components.Routing;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.Web.Virtualization;
using Microsoft.JSInterop;
using BlazorApp1;
using BlazorApp1.Shared;
using Proto;
using Proto.Cluster;

namespace BlazorApp1.Pages;

public partial class Counter
{
    [Inject]
    private IRootContext Root { get; init;}

    private int currentCount = 0;
    private void IncrementCount()
    {
        using var cts = new CancellationTokenSource();
        currentCount++;
    }

    protected override async Task OnInitializedAsync()
    {
        using var cts = new CancellationTokenSource();

        currentCount = await Root.System.Cluster().RequestAsync<int>("1", "CounterActor", "OnInitializedAsync", cts.Token);
        StateHasChanged();

        await base.OnInitializedAsync();
    }
}

public class CounterActor : IActor
{
    private int Current { get; set; } = 0;
    public Task ReceiveAsync(IContext context) => context.Message switch
    {
        "OnInitializedAsync" => Task.Run(() => context.Respond(Current)),

    };
}
