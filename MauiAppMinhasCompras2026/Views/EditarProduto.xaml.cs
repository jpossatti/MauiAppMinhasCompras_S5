using MauiAppMinhasCompras2026.Models;
using System.Globalization;

namespace MauiAppMinhasCompras2026.Views;

public partial class EditarProduto : ContentPage
{
    public EditarProduto()
    {
        InitializeComponent();
    }

    private async void ToolbarItem_Clicked(object sender, EventArgs e)
    {
        try
        {
            // Recupera o produto anexo via BindingContext (Padrão ETEC Agenda 05)
            Produto produto_anexado = BindingContext as Produto;

            if (produto_anexado != null)
            {
                // Validação de preenchimento dos campos
                if (string.IsNullOrWhiteSpace(txt_descricao.Text))
                {
                    await DisplayAlert("Atenção", "Por favor, informe a descrição do produto.", "OK");
                    txt_descricao.Focus();
                    return;
                }

                if (string.IsNullOrWhiteSpace(txt_quantidade.Text))
                {
                    await DisplayAlert("Atenção", "Por favor, informe a quantidade.", "OK");
                    txt_quantidade.Focus();
                    return;
                }

                if (string.IsNullOrWhiteSpace(txt_preco.Text))
                {
                    await DisplayAlert("Atenção", "Por favor, informe o preço unitário.", "OK");
                    txt_preco.Focus();
                    return;
                }

                // Tratamento para aceitar vírgula ou ponto decimal
                string qteTexto = txt_quantidade.Text.Replace(',', '.');
                string precoTexto = txt_preco.Text.Replace(',', '.');

                if (!double.TryParse(qteTexto, NumberStyles.Any, CultureInfo.InvariantCulture, out double quantidade))
                {
                    await DisplayAlert("Atenção", "Quantidade inválida.", "OK");
                    txt_quantidade.Focus();
                    return;
                }

                if (!double.TryParse(precoTexto, NumberStyles.Any, CultureInfo.InvariantCulture, out double preco))
                {
                    await DisplayAlert("Atenção", "Preço inválido.", "OK");
                    txt_preco.Focus();
                    return;
                }

                // Atualiza o objeto com os novos valores
                produto_anexado.Descricao = txt_descricao.Text.Trim();
                produto_anexado.Quantidade = quantidade;
                produto_anexado.Preco = preco;

                // Executa a atualização no Banco de Dados
                await App.Db.Update(produto_anexado);

                await DisplayAlert("Sucesso!", "Produto atualizado com sucesso.", "OK");

                // Retorna para a tela de listagem
                await Navigation.PopAsync();
            }
        }
        catch (Exception ex)
        {
            await DisplayAlert("Ops", $"Erro ao atualizar: {ex.Message}", "OK");
        }
    }
}