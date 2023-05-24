using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Global_Settings;
using Microsoft.EntityFrameworkCore;
using Services.Interfaces;
using ViewModels.PriceLists;

namespace Services
{
    public class TPriceListsService : ITPriceListsService
    {
        public async Task<IEnumerable<PriceListTableViewModel>> GetAllPriceListstForDataObjectType(int dataObjectId, int dataObjectType, bool forExternalDisplayOnly)
        {
            var priceLists = new List<PriceListTableViewModel>();
            string query = String.Empty;

            if (dataObjectType == ((int)Enumerations.DataObjectTypes.Org))
            {
                query = @"select ClientPriceLists.ID, ClientPricelists.Name, LocalCurrencyInfo.CurrencyName, InForceSinceDateTime, DataObjectTypeID, DataObjectID, Orgs.OrgName from ClientPriceLists

                    inner join LocalCurrencyInfo on LocalCurrencyInfo.CurrencyID = ClientPriceLists.CurrencyID
					 left outer join Orgs on Orgs.ID = DataObjectID
                    WHERE	((DataObjectID = " + dataObjectId + @"
		            AND	DataObjectTypeID = " + dataObjectType + @")
		            OR (DataObjectID IN (SELECT ID FROM [dbo].[Contacts] WHERE OrgID = " + dataObjectId + @" AND DeletedDate IS NULL)
		            AND	DataObjectTypeID = 1 --contacts
		            ))
		            AND	NoLongerInForceAsOfDateTime IS NULL
		            ORDER BY DataObjectTypeID DESC, ClientPricelists.Name";
            }
            else if (dataObjectType == ((int)Enumerations.DataObjectTypes.Contact))
            {
                
                query = @"select ClientPriceLists.ID, ClientPricelists.Name, LocalCurrencyInfo.CurrencyName, InForceSinceDateTime, DataObjectTypeID, DataObjectID, Contacts.Name from ClientPriceLists

                    inner join LocalCurrencyInfo on LocalCurrencyInfo.CurrencyID = ClientPriceLists.CurrencyID
					 left outer join Contacts on Contacts.ID = DataObjectID
                    WHERE	((DataObjectID = " + dataObjectId + @"
		            AND	DataObjectTypeID = " + dataObjectType + @")
		            OR (DataObjectID IN (SELECT ID FROM [dbo].[Contacts] WHERE ID = " + dataObjectId + @" AND DeletedDate IS NULL)
		            AND	DataObjectTypeID = 1 --contacts
		            ))
		            AND	NoLongerInForceAsOfDateTime IS NULL
		            ORDER BY DataObjectTypeID DESC, ClientPricelists.Name";
            }
            else if (dataObjectType == ((int)Enumerations.DataObjectTypes.OrgGroup))
            {
                query = @"select ClientPriceLists.ID, ClientPricelists.Name, LocalCurrencyInfo.CurrencyName, InForceSinceDateTime, DataObjectTypeID, DataObjectID, OrgGroups.Name from ClientPriceLists

                    inner join LocalCurrencyInfo on LocalCurrencyInfo.CurrencyID = ClientPriceLists.CurrencyID
					 left outer join OrgGroups on OrgGroups.ID = DataObjectID
                    WHERE	((DataObjectID = " + dataObjectId + @"
		            AND	DataObjectTypeID = " + dataObjectType + @")
		            OR (DataObjectID IN (SELECT ID FROM [dbo].[OrgGroups] WHERE ID = " + dataObjectId + @" AND DeletedDate IS NULL)
		            AND	DataObjectTypeID = 1 --contacts
		            ))
		            AND	NoLongerInForceAsOfDateTime IS NULL
		            ORDER BY DataObjectTypeID DESC, ClientPricelists.Name";
            }



            using (var context = new Data.TPCoreProductionContext())
            {
                using var command = context.Database.GetDbConnection().CreateCommand();
                command.CommandText = query;
                context.Database.OpenConnection();
                using var result = command.ExecuteReader();

                while (await result.ReadAsync())
                {
                    priceLists.Add(new PriceListTableViewModel()
                    {
                        Id = await result.GetFieldValueAsync<int>(0),
                        Name = await result.GetFieldValueAsync<string>(1),
                        CurrencyName = await result.GetFieldValueAsync<string>(2),
                        InForce = await result.GetFieldValueAsync<DateTime>(3),
                        AppliesToDataObjectType = await result.GetFieldValueAsync<byte>(4),
                        AppliesToDataObjectId = await result.GetFieldValueAsync<int>(5),
                        AppliesToName = result.IsDBNull(6) == true ? "" : await result.GetFieldValueAsync<string>(6)
                    });
                }
            }
            return priceLists;
        }
    }
}
