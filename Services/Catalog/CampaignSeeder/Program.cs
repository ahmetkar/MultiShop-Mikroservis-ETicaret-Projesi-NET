using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Driver;

Console.WriteLine("Connecting to MongoDB...");
var client = new MongoClient("mongodb://localhost:27017");
var db = client.GetDatabase("MultiShopCatalogDb");

var productsCollection = db.GetCollection<ProductDoc>("Products");
var allProducts = await productsCollection.Find(x => true).ToListAsync();
Console.WriteLine($"Total products found: {allProducts.Count}");

if (allProducts.Count > 0)
{
    var sliderCol = db.GetCollection<SliderDoc>("FeatureSliders");
    var sliders = await sliderCol.Find(x => true).ToListAsync();
    Console.WriteLine($"Found {sliders.Count} sliders.");
    for (int i = 0; i < sliders.Count; i++)
    {
        var takeProducts = allProducts.Skip(i * 6).Take(8).Select(p => p.ProductId).ToList();
        var filter = Builders<SliderDoc>.Filter.Eq(x => x.FeatureSliderID, sliders[i].FeatureSliderID);
        var update = Builders<SliderDoc>.Update.Set(x => x.ProductIds, takeProducts).Set(x => x.Status, true);
        await sliderCol.UpdateOneAsync(filter, update);
        Console.WriteLine($"Slider '{sliders[i].Title}' linked with {takeProducts.Count} products.");
    }

    var specialCol = db.GetCollection<SpecialDoc>("SpecialOffers");
    var specials = await specialCol.Find(x => true).ToListAsync();
    Console.WriteLine($"Found {specials.Count} special offers.");
    for (int i = 0; i < specials.Count; i++)
    {
        var takeProducts = allProducts.Skip(20 + (i * 6)).Take(6).Select(p => p.ProductId).ToList();
        var filter = Builders<SpecialDoc>.Filter.Eq(x => x.SpecialOfferId, specials[i].SpecialOfferId);
        var update = Builders<SpecialDoc>.Update.Set(x => x.ProductIds, takeProducts);
        await specialCol.UpdateOneAsync(filter, update);
        Console.WriteLine($"Special Offer '{specials[i].Title}' linked with {takeProducts.Count} products.");
    }

    var offerCol = db.GetCollection<OfferDoc>("OfferDiscounts");
    var offers = await offerCol.Find(x => true).ToListAsync();
    Console.WriteLine($"Found {offers.Count} offer discounts.");
    for (int i = 0; i < offers.Count; i++)
    {
        var takeProducts = allProducts.Skip(40 + (i * 6)).Take(6).Select(p => p.ProductId).ToList();
        var filter = Builders<OfferDoc>.Filter.Eq(x => x.OfferDiscountId, offers[i].OfferDiscountId);
        var update = Builders<OfferDoc>.Update.Set(x => x.ProductIds, takeProducts);
        await offerCol.UpdateOneAsync(filter, update);
        Console.WriteLine($"Offer Discount '{offers[i].Title}' linked with {takeProducts.Count} products.");
    }
}

Console.WriteLine("Done!");

class ProductDoc
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string ProductId { get; set; }
    public string ProductName { get; set; }
}

class SliderDoc
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string FeatureSliderID { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public string ImageUrl { get; set; }
    public bool Status { get; set; }
    public List<string> ProductIds { get; set; } = new List<string>();
}

class SpecialDoc
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string SpecialOfferId { get; set; }
    public string Title { get; set; }
    public string Subtitle { get; set; }
    public string ImageUrl { get; set; }
    public List<string> ProductIds { get; set; } = new List<string>();
}

class OfferDoc
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string OfferDiscountId { get; set; }
    public string Title { get; set; }
    public string Subtitle { get; set; }
    public string ImageUrl { get; set; }
    public string ButtonTitle { get; set; }
    public List<string> ProductIds { get; set; } = new List<string>();
}

