// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using IdentityServer4;
using IdentityServer4.Models;
using System.Collections.Generic;

namespace MultiShop.IdentityServer
{
    public static class Config
    {

        public static IEnumerable<ApiResource> ApiResources =>
        new ApiResource[]
        {
            new ApiResource("ResourceCatalog")
            {
                Scopes = {"CatalogFullPermission","CatalogReadPermission"}
            },
            new ApiResource("ResourceDiscount")
            {
                Scopes = {"DiscountFullPermission"}
            }, 
            new ApiResource("ResourceOrder")
            {
                Scopes = {"OrderFullPermission"}
            },
            new ApiResource(IdentityServerConstants.LocalApi.ScopeName),
            new ApiResource("ResourceCargo")
            {
                Scopes = {"CargoFullPermission"}
            },
            new ApiResource("ResourceBasket")
            {
                Scopes = {"BasketFullPermission"}
            },
            new ApiResource("ResourceOcelot")
            {
                Scopes = {"OcelotFullPermission"}
            },
            new ApiResource("ResourceComment")
            {
                Scopes = {"CommentFullPermission"}
            },
            new ApiResource("ResourcePayment")
            {
                Scopes =
                {
                    "PaymentReadPermission",
                    "PaymentCreatePermission",
                    "PaymentUpdatePermission",
                    "PaymentDeletePermission",
                    "PaymentFullPermission"
                }
            },
            new ApiResource("ResourceImages")
            {
                Scopes = {"ImagesFullPermission"}
            },
            new ApiResource("ResourceMessage")
            {
                Scopes = {"MessageFullPermission"}
            }
        };

        public static IEnumerable<IdentityResource> IdentityResources =>
        new IdentityResource[]
        {
            new IdentityResources.OpenId(),
            new IdentityResources.Email(),
            new IdentityResources.Profile(),
            new IdentityResource("roles", "Kullanıcı Rolleri", new[] { "role" })
        };

        public static IEnumerable<ApiScope> ApiScopes =>
            new ApiScope[]
            {
                new ApiScope("CatalogFullPermission","Full authority for catalog operations"),
                new ApiScope("CatalogReadPermission","Reading authority for catalog operations"),
                new ApiScope("DiscountFullPermission","Full authority for discount"),
                new ApiScope("OrderFullPermission","Full authority for order operations"),
                new ApiScope("CargoFullPermission","Full authority for cargo operations"),
                new ApiScope("BasketFullPermission","Full authority for basket operations"),
                new ApiScope("OcelotFullPermission","Full authority for ocelot operations"),
                new ApiScope("CommentFullPermission","Full authority for comment operations"),
                new ApiScope("PaymentReadPermission", "Can read payment records"),
                new ApiScope("PaymentCreatePermission", "Can create payment records"),
                new ApiScope("PaymentUpdatePermission", "Can update payment records"),
                new ApiScope("PaymentDeletePermission", "Can delete payment records"),
                new ApiScope("PaymentFullPermission", "Full authority for payment operations"),
                new ApiScope("ImagesFullPermission","Full authority for image operations"),
                new ApiScope("MessageFullPermission","Full authority for message operations"),
                new ApiScope(IdentityServerConstants.LocalApi.ScopeName)
            };

        public static IEnumerable<Client> Clients =>
            new Client[]
            {
                //Visitor
                new Client { 
                    ClientId = "MultiShopVisitorId",
                    ClientName="MultiShopVisitorUser",
                    AllowedGrantTypes = GrantTypes.ClientCredentials,
                    ClientSecrets = {new Secret("multishopsecret".Sha256())},
                    AllowedScopes = { "PaymentReadPermission", "PaymentCreatePermission", "PaymentDeletePermission", "CatalogReadPermission", "CatalogFullPermission", "OcelotFullPermission","CommentFullPermission", "PaymentFullPermission", "DiscountFullPermission", "CargoFullPermission", "OrderFullPermission", IdentityServerConstants.LocalApi.ScopeName },
                    AllowAccessTokensViaBrowser = true
                },
                //User (Member / Normal User)
                new Client
                {
                    ClientId = "MultiShopUserId",
                    ClientName = "MultiShopUserClient",
                    AllowedGrantTypes = GrantTypes.ResourceOwnerPassword,
                    ClientSecrets = { new Secret("multishopsecret".Sha256()) },
                    AllowOfflineAccess = true,
                    AlwaysIncludeUserClaimsInIdToken = true,
                    AllowedScopes = {
                        "CatalogReadPermission", "CatalogFullPermission", "BasketFullPermission", "OcelotFullPermission",
                        "CommentFullPermission", "DiscountFullPermission", "OrderFullPermission", "CargoFullPermission",
                        "PaymentReadPermission", "PaymentCreatePermission", "PaymentUpdatePermission", "PaymentDeletePermission", "PaymentFullPermission", "roles",
                        IdentityServerConstants.LocalApi.ScopeName,
                        IdentityServerConstants.StandardScopes.Email,
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile,
                        IdentityServerConstants.StandardScopes.OfflineAccess
                    },
                    AccessTokenLifetime = 600
                },
                //Manager
                new Client
                {
                    ClientId = "MultiShopManagerId",
                    ClientName="MultiShopManagerUser",
                    AllowedGrantTypes = GrantTypes.ResourceOwnerPassword,
                    ClientSecrets = {new Secret("multishopsecret".Sha256())},
                    AllowOfflineAccess = true,
                    AlwaysIncludeUserClaimsInIdToken = true,
                    AllowedScopes = {
                        "PaymentReadPermission", "PaymentCreatePermission", "PaymentDeletePermission", "PaymentUpdatePermission",
                        "CatalogFullPermission", "CatalogReadPermission", "BasketFullPermission", "OcelotFullPermission",
                        "PaymentFullPermission", "CommentFullPermission", "ImagesFullPermission", "DiscountFullPermission",
                        "MessageFullPermission", "CargoFullPermission", "OrderFullPermission", "roles",
                        IdentityServerConstants.LocalApi.ScopeName,
                        IdentityServerConstants.StandardScopes.Email,
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile,
                        IdentityServerConstants.StandardScopes.OfflineAccess
                    }
                },
                new Client
                {
                    ClientId = "MultiShopAdminId",
                    ClientName = "MultiShopAdminUser",
                    AllowedGrantTypes = GrantTypes.ResourceOwnerPassword,
                    ClientSecrets = {new Secret("multishopsecret".Sha256())},
                    AllowOfflineAccess = true,
                    AlwaysIncludeUserClaimsInIdToken = true,
                    AllowedScopes = { 
                        "CatalogFullPermission", "CatalogReadPermission", 
                        "DiscountFullPermission", "OrderFullPermission",
                        "CargoFullPermission", "BasketFullPermission", "OcelotFullPermission", "CommentFullPermission", "PaymentFullPermission",
                        "ImagesFullPermission", "MessageFullPermission", "roles",
                        "PaymentReadPermission", "PaymentCreatePermission", "PaymentDeletePermission", "PaymentUpdatePermission",
                        IdentityServerConstants.LocalApi.ScopeName,
                        IdentityServerConstants.StandardScopes.Email,
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile,
                        IdentityServerConstants.StandardScopes.OfflineAccess
                    },    
                    AccessTokenLifetime = 600
                }
            };
    }
}