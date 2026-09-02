using MauiAppMinhasCompras2026.Models;
using System.Collections.ObjectModel;

namespace MauiAppMinhasCompras2026.Views;

public partial class ListaProduto : ContentPage
{
    private ObservableCollection<Produto> _lista = new ObservableCollection<Produto>();

    public ListaProduto()
    {
        InitializeComponent();
        lst_produtos.ItemsSource = _lista;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await CarregarProdutos();
    }

    private async Task CarregarProdutos()
    {
        try
        {
            _lista.Clear();
            List<Produto> tmp = await App.Db.GetAll();
            foreach (Produto p in tmp)
            {
                _lista.Add(p);
            }

            CalcularTotalGeral();
        }
        catch (Exception ex)
        {
            await DisplayAlert("Ops", ex.Message, "OK");
        }
    }

    private void CalcularTotalGeral()
    {
        double total = _lista.Sum(i => i.Total);
        lblTotalGeral.Text = total.ToString("C");
    }

    private async void ToolbarItem_Clicked(object sender, EventArgs e)
    {
        // Abre a tela de cadastro
        await Navigation.PushAsync(new NovoProduto());
    }

    private async void ToolbarItem_Clicked_1(object sender, EventArgs e)
    {
        // Exibe o alerta com o somatório
        double total = _lista.Sum(i => i.Total);
        await DisplayAlert("Total das Compras", $"O valor total é {total:C}", "OK");
    }

    private async void lst_produtos_ItemSelected(object sender, SelectedItemChangedEventArgs e)
    {
        try
        {
            Produto p = e.SelectedItem as Produto;

            if (p != null)
            {
                // Instancia a tela de edição e define o BindingContext (Padrão ETEC Agenda 05)
                EditarProduto telaEdicao = new EditarProduto();
                telaEdicao.BindingContext = p;

                // Desmarca o item da lista
                lst_produtos.SelectedItem = null;

                await Navigation.PushAsync(telaEdicao);
            }
        }
        catch (Exception ex)
        {
            await DisplayAlert("Ops", ex.Message, "OK");
        }
    }

    private async void MenuItem_Clicked(object sender, EventArgs e)
    {
        try
        {
            MenuItem mi = sender as MenuItem;
            Produto p = mi.BindingContext as Produto;

            bool confirma = await DisplayAlert("Tem Certeza?", $"Remover {p.Descricao}?", "Sim", "Não");

            if (confirma)
            {
                await App.Db.Delete(p.Id);
                await CarregarProdutos();
            }
        }
        catch (Exception ex)
        {
            await DisplayAlert("Ops", ex.Message, "OK");
        }
    }

    private async void txt_search_TextChanged(object sender, TextChangedEventArgs e)
    {
        try
        {
            string q = e.NewTextValue;

            _lista.Clear();
            List<Produto> tmp = await App.Db.Search(q);
            foreach (Produto p in tmp)
            {
                _lista.Add(p);
            }

            CalcularTotalGeral();
        }
        catch (Exception ex)
        {
            await DisplayAlert("Ops", ex.Message, "OK");
        }
    }

    private async void lst_produtos_Refreshing(object sender, EventArgs e)
    {
        await CarregarProdutos();
        lst_produtos.IsRefreshing = false;
    }
}