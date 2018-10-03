using Assignment2WebApi.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;

namespace Assignment2WebApi.Models
{
    public static class SeedData
    {
        public static void Initialize(IServiceProvider serviceProvider)
        {
            using (var context = new ApplicationDbContext(
                serviceProvider.GetRequiredService<DbContextOptions<ApplicationDbContext>>()))
            {
                // Look for any products.
                if (context.Products.Any())
                {
                    return; // DB has been seeded.
                }

                var products = new[]
                {
                    new Product
                    {
                        Name = "Rabbit",
                        Price = 50
                    },
                    new Product
                    {
                        Name = "Hat",
                        Price = 25
                    },
                    new Product
                    {
                        Name = "Svengali Deck",
                        Price = 50.95
                    },
                    new Product
                    {
                        Name = "Floating Hankerchief",
                        Price = 50.50
                    },
                    new Product
                    {
                        Name = "Wand",
                        Price = 150.25
                    },
                    new Product
                    {
                        Name = "Broomstick",
                        Price = 200
                    },
                    new Product
                    {
                        Name = "Bang Gun",
                        Price = 300.99
                    },
                    new Product
                    {
                        Name = "Cloak of Invisibility",
                        Price = 1000
                    },
                    new Product
                    {
                        Name = "Elder Wand",
                        Price = 1000
                    },
                    new Product
                    {
                        Name = "Resurrection Stone",
                        Price = 1000
                    }
                };

                context.Products.AddRange(products);

                var i = 0;
                context.OwnerInventory.AddRange(
                    new OwnerInventory
                    {
                        Product = products[i++],
                        StockLevel = 20
                    },
                    new OwnerInventory
                    {
                        Product = products[i++],
                        StockLevel = 50
                    },
                    new OwnerInventory
                    {
                        Product = products[i++],
                        StockLevel = 100
                    },
                    new OwnerInventory
                    {
                        Product = products[i++],
                        StockLevel = 150
                    },
                    new OwnerInventory
                    {
                        Product = products[i++],
                        StockLevel = 40
                    },
                    new OwnerInventory
                    {
                        Product = products[i++],
                        StockLevel = 10
                    },
                    new OwnerInventory
                    {
                        Product = products[i++],
                        StockLevel = 5
                    },
                    new OwnerInventory
                    {
                        Product = products[i++],
                        StockLevel = 0
                    },
                    new OwnerInventory
                    {
                        Product = products[i++],
                        StockLevel = 0
                    },
                    new OwnerInventory
                    {
                        Product = products[i],
                        StockLevel = 0
                    }
                );

                i = 0;
                var stores = new[]
                {
                    new Store
                    {
                        Name = "Melbourne CBD",
                        StoreInventory =
                        {
                            new StoreInventory
                            {
                                Product = products[i++],
                                StockLevel = 15
                            },
                            new StoreInventory
                            {
                                Product = products[i++],
                                StockLevel = 10
                            },
                            new StoreInventory
                            {
                                Product = products[i++],
                                StockLevel = 5
                            },
                            new StoreInventory
                            {
                                Product = products[i++],
                                StockLevel = 5
                            },
                            new StoreInventory
                            {
                                Product = products[i++],
                                StockLevel = 5
                            },
                            new StoreInventory
                            {
                                Product = products[i++],
                                StockLevel = 5
                            },
                            new StoreInventory
                            {
                                Product = products[i++],
                                StockLevel = 5
                            },
                            new StoreInventory
                            {
                                Product = products[i++],
                                StockLevel = 1
                            },
                            new StoreInventory
                            {
                                Product = products[i++],
                                StockLevel = 1
                            },
                            new StoreInventory
                            {
                                Product = products[i],
                                StockLevel = 1
                            },
                        }
                    },
                    new Store
                    {
                        Name = "North Melbourne",
                        StoreInventory =
                        {
                            new StoreInventory
                            {
                                Product = products[0],
                                StockLevel = 5
                            }
                        }
                    },
                    new Store
                    {
                        Name = "East Melbourne",
                        StoreInventory =
                        {
                            new StoreInventory
                            {
                                Product = products[1],
                                StockLevel = 5
                            }
                        }
                    },
                    new Store
                    {
                        Name = "South Melbourne",
                        StoreInventory =
                        {
                            new StoreInventory
                            {
                                Product = products[2],
                                StockLevel = 5
                            }
                        }
                    },
                    new Store
                    {
                        Name = "West Melbourne"
                    }
                };

                context.Stores.AddRange(stores);

                context.SaveChanges();
            }
        }
    }
}

