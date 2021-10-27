using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;

namespace Blasmec.Blazor
{
    public partial class AutocompleteSearch <TItem>
    {
        [Parameter] public TItem SelectedItem
        {
            get => _selectedItem;
            set
            {
                _selectedItem = value;
                data.Add(_selectedItem);
            }
        }
        [Parameter] public EventCallback<TItem> SelectedItemChanged { get; set; }
        [Parameter] public Func<TItem, string> TextField { get; set; }
        [Parameter] public Func<string, List<TItem>> OnSearch { get; set; }
        [Parameter] public Func<TItem> OnInit { get; set; }
        [Parameter] public Func<TItem> OnAfterRendering { get; set; }
        [Parameter] public string Placeholder { get; set; }

        private List<TItem> data = new List<TItem>();
        private TItem _selectedItem;


        private async Task _updateSelectedItem(TItem item)
        {
            SelectedItem = item;
            await SelectedItemChanged.InvokeAsync(item);
        }
        protected override void OnInitialized()
        {
            if (OnInit is null) return;
            var item = OnInit.Invoke();
            data.Add(item);
            SelectedItem = item;
        }

        protected override void OnAfterRender(bool firstRender)
        {
            if (!firstRender) return;
            if (OnAfterRendering is null) return;
            var item = OnAfterRendering.Invoke();
            data.Add(item);
            SelectedItem = item;
            StateHasChanged();
        }

        private void _onSearch(string searchText)
        {
            if (searchText is null) return;

            var items = OnSearch?.Invoke(searchText) ?? new List<TItem>();
            data.Clear();
            data.AddRange(items);
        }
    }
}
