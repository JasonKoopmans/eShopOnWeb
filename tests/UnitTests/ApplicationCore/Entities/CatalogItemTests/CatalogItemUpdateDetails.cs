using Microsoft.eShopWeb.ApplicationCore.Entities;
using Xunit;

namespace Microsoft.eShopWeb.UnitTests.ApplicationCore.Entities.CatalogItemTests;

public class CatalogItemUpdateDetails
{
    private const string OriginalName = "name";
    private const string OriginalDescription = "desc";
    private const decimal OriginalPrice = 1.23m;

    private static CatalogItem CreateItem() =>
        new CatalogItem(1, 1, OriginalDescription, OriginalName, OriginalPrice, "pic");

    private static void AssertUnchanged(CatalogItem item)
    {
        Assert.Equal(OriginalName, item.Name);
        Assert.Equal(OriginalDescription, item.Description);
        Assert.Equal(OriginalPrice, item.Price);
    }

    [Fact]
    public void ThrowsGivenEmptyName()
    {
        var item = CreateItem();

        Assert.Throws<ArgumentException>(() =>
            item.UpdateDetails(new CatalogItem.CatalogItemDetails(string.Empty, "new desc", 2m)));
        AssertUnchanged(item);
    }

    [Fact]
    public void ThrowsGivenEmptyDescription()
    {
        var item = CreateItem();

        Assert.Throws<ArgumentException>(() =>
            item.UpdateDetails(new CatalogItem.CatalogItemDetails("new name", string.Empty, 2m)));
        AssertUnchanged(item);
    }

    [Fact]
    public void ThrowsGivenZeroPrice()
    {
        var item = CreateItem();

        Assert.Throws<ArgumentException>(() =>
            item.UpdateDetails(new CatalogItem.CatalogItemDetails("new name", "new desc", 0m)));
        AssertUnchanged(item);
    }

    [Fact]
    public void ThrowsGivenNegativePrice()
    {
        var item = CreateItem();

        Assert.Throws<ArgumentException>(() =>
            item.UpdateDetails(new CatalogItem.CatalogItemDetails("new name", "new desc", -1m)));
        AssertUnchanged(item);
    }

    [Fact]
    public void UpdatesNameDescriptionAndPriceGivenValidDetails()
    {
        var item = CreateItem();

        item.UpdateDetails(new CatalogItem.CatalogItemDetails("new name", "new desc", 4.56m));

        Assert.Equal("new name", item.Name);
        Assert.Equal("new desc", item.Description);
        Assert.Equal(4.56m, item.Price);
    }
}
