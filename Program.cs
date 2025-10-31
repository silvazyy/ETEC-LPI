
using System.Text.RegularExpressions;
using App.Core.Model;
string path = "C:\\ph e nk\\FacilitadorDeCompraApp\\lista_de_produto_para_comprar-2.csv";
List<ProdutoModel> produtoList = new();
try
{
    StreamReader str = new StreamReader(path);
    string? line = str.ReadLine();
    while (line != null)
    {
        Console.WriteLine(line);
        line = str.ReadLine();
        ProdutoModel? prodmod = ProdutoModel.ConvertCsvTextLineToModel(line);
        if (prodmod != null)
        {
            produtoList.Add(prodmod);
        }
    }
    Console.WriteLine(str.ReadLine());
}
catch (Exception ex)
{
    Console.WriteLine(ex.Message);
}
Console.WriteLine(produtoList.Count());
Dictionary<string, MercadoModel> mercadoDictionary = new();

foreach (ProdutoModel produto in produtoList)
{
    if (mercadoDictionary.ContainsKey(produto.MercadoNome) == false)
    {
        mercadoDictionary.Add(produto.MercadoNome, new MercadoModel()
        {
            Nome = produto.MercadoNome
        });
    }

    MercadoModel mercadoModel = mercadoDictionary[produto.MercadoNome];

    ProdutoModel produtoSelecionado = ComparadorProdutoMercadoModel.ProdutoComMenorPreco(produtoList, produto.Ean);

    if (produtoSelecionado.MercadoNome.Equals(mercadoModel.Nome))
    {
        mercadoModel.ProdutoList.Add(produtoSelecionado);
    }
    
}


foreach (MercadoModel mercadoModel1 in mercadoDictionary.Values)
{
    StreamWriter streamWriter = new StreamWriter($"\\ph e nk\\FacilitadorDeCompraApp\\ListaDeCompras{Regex.Replace(mercadoModel1.Nome, "[^A-Za-z0-9]", "_")}.txt");

    foreach (ProdutoModel produto in mercadoModel1.ProdutoList)
    {
        streamWriter.WriteLine($"Produto: {produto.Nome} custa R${produto.Preco} ({produto.UnidadeDeVendas})");
        streamWriter.Flush();
    }
    streamWriter.Close();
}