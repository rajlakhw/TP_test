using System.Collections.Generic;
using System.Linq;
using Services.Interfaces;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Data;
using Data.Repositories;
using ViewModels.Common;
using System;

namespace Services
{
    public class OrgGroupService : IOrgGroupService
    {
        private readonly IRepository<OrgGroup> orgGroupRepository;
        private readonly IRepository<Org> orgRepository;
        private readonly IRepository<Contact> contactRepository;
        private readonly IRepository<JobOrder> orderRepository;
        private readonly IRepository<AltairCorporateGroupe> altairGroupeRepository;

        public OrgGroupService(IRepository<OrgGroup> repository, IRepository<Org> repository1, IRepository<Contact> repository2,
                               IRepository<JobOrder> repository3, IRepository<AltairCorporateGroupe> repository4)
        {
            this.orgGroupRepository = repository;
            this.orgRepository = repository1;
            this.contactRepository = repository2;
            this.orderRepository = repository3;
            this.altairGroupeRepository = repository4;
        }

        public async Task<IEnumerable<DropdownOptionViewModel>> GetAllAltairCorporateGroups() => await altairGroupeRepository.All().Select(x => new DropdownOptionViewModel() { Id = x.Id, Name = x.Name }).ToListAsync();

        public async Task<Dictionary<int, string>> GetAllOrgGroupsIdAndName(bool onlyGetGroupWithJobs = true)
        {
            Dictionary<int, string> result = null;
            if (onlyGetGroupWithJobs == true)
            {
                result = await orderRepository.All().Where(o => o.DeletedDate == null).Select(o => o.ContactId)
                                                   .Join(contactRepository.All(),
                                                   o => o,
                                                   c => c.Id,
                                                   (o, c) => new { contact = c })
                                                   .Where(c => c.contact.DeletedDate == null)
                                                   .Select(c => c.contact.OrgId).Distinct()
                                                   .Join(orgRepository.All(),
                                                   c => c,
                                                   o => o.Id,
                                                   (c, o) => new { org = o })
                                                   .Where(o => o.org.DeletedDate == null)
                                                   .Select(o => o.org.OrgGroupId).Distinct()
                                                   .Join(orgGroupRepository.All(),
                                                   o => o,
                                                   og => og.Id,
                                                   (o, og) => new { group = og })
                                                   .Where(g => g.group.DeletedDate == null)
                                                   .Select(g => new { g.group.Id, g.group.Name}).OrderBy(o => o.Name).ToDictionaryAsync(i => i.Id, j => j.Name);
            }
            else
            {
                result = await orgGroupRepository.All().Where(o => o.DeletedDate == null).Select(o => new { o.Id, o.Name}).OrderBy(o => o.Name).ToDictionaryAsync(i => i.Id, j => j.Name);
            }

            return result;
        }


        public async Task<List<string>> GetAllGroupsForInternalExternalFilters(string InternalExternalOrAll)
        {
            string[] externalGroupIDs = { "72112", "72113", "72114", "72115" };
            if (InternalExternalOrAll == "internal")
            {
                var result = await orderRepository.All().Where(o => o.DeletedDate == null).Select(o => o.ContactId)
                                    .Join(contactRepository.All().Where(c => c.DeletedDate == null),
                                    o => o,
                                    c => c.Id,
                                    (o, c) => new { orgId = c.OrgId }).Distinct()
                                    .Join(orgRepository.All().Where(o => o.DeletedDate == null),
                                    c => c.orgId,
                                    o => o.Id,
                                    (c, o) => new { orgGroupId = o.OrgGroupId }).Distinct()
                                    .Join(orgGroupRepository.All(),
                                    o => o.orgGroupId,
                                    g => g.Id,
                                    (o,g) => new {group = g }).Distinct()
                                    .Where(g => g.group.DeletedDate == null && !externalGroupIDs.Contains(g.group.Id.ToString()))
                                    .Select(og => new { customName = og.group.Id + " - " + og.group.Name, GroupName = og.group.Name }).Distinct()
                                    .OrderBy(og => og.GroupName).Select(o => o.customName)
                                    .ToListAsync();

                return result;
            }
            else if (InternalExternalOrAll == "external")
            {
                var result = await orgGroupRepository.All().Where(o => o.DeletedDate == null && externalGroupIDs.Contains(o.Id.ToString()))
                                    .Join(orgRepository.All().Where(o => o.DeletedDate == null),
                                     g => g.Id,
                                     o => o.OrgGroupId,
                                     (g, o) => new { group = g, orgId = o.Id }).Distinct()
                                    .Join(contactRepository.All().Where(c => c.DeletedDate == null),
                                     o => o.orgId,
                                     c => c.OrgId,
                                     (o, c) => new { contactId = c.Id, group = o.group }).Distinct()
                                    .Join(orderRepository.All().Where(o => o.DeletedDate == null),
                                     c => c.contactId,
                                     o => o.ContactId,
                                     (c, o) => new { customName = c.group.Id.ToString() + " - " + c.group.Name, Name = c.group.Name }).Distinct()
                                    .OrderBy(og => og.Name).Select(o => o.customName)
                                    .ToListAsync();

                //var result = await orgGroupRepository.All().Where(o => o.DeletedDate == null && externalGroupIDs.Contains(o.Id.ToString()))
                //                    .Select(og => new { customName = og.Id.ToString() + " - " + og.Name, Name = og.Name })
                //                    .OrderBy(og => og.Name).Select(o => o.customName)
                //                    .ToListAsync();
                return result;
            }
            else
            {
                var result = await orderRepository.All().Where(o => o.DeletedDate == null).Select(o => o.ContactId)
                                     .Join(contactRepository.All().Where(c => c.DeletedDate == null),
                                     o => o,
                                     c => c.Id,
                                     (o, c) => new { orgId = c.OrgId }).Distinct()
                                     .Join(orgRepository.All().Where(o => o.DeletedDate == null),
                                     c => c.orgId,
                                     o => o.Id,
                                     (c, o) => new { groupId = o.OrgGroupId }).Distinct()
                                     .Join(orgGroupRepository.All().Where(g => g.DeletedDate == null),
                                     o => o.groupId,
                                     g => g.Id,
                                     (o, g) => new { customName = g.Id + " - " + g.Name, GroupName = g.Name }).Distinct()
                                     .OrderBy(og => og.GroupName).Select(o => o.customName)
                                     .ToListAsync();

                return result;
            }



        }


        //public async Task<List<string>> GetAllOrgGroupsIdAndName(string orgGroupIDOrNameToSearch)
        //{

        //    if (orgGroupIDOrNameToSearch.All(char.IsNumber) == true)
        //    {
        //        var result = await orgGroupRepository.All().Where(o => o.Id.ToString().StartsWith(orgGroupIDOrNameToSearch) && o.DeletedDate == null)
        //                                              .Select(o => String.Join(" - ", o.Id.ToString(), o.Name)).ToListAsync();
        //        return result;
        //    }
        //    else
        //    {
        //        var result = await orgGroupRepository.All()
        //                            .Where(o => o.Name.StartsWith(orgGroupIDOrNameToSearch) && o.DeletedDate == null)
        //                            .Select(o => String.Join(" - ", o.Id.ToString(), o.Name)).ToListAsync();
        //        return result;

        //    }

        //    //Dictionary<int, string> result = await orgGroupRepository.All().Select(o => new { o.Id, o.Name, o.DeletedDate }).Where(o => o.DeletedDate == null).ToDictionaryAsync(i => i.Id, j => j.Name);

        //    //return result;
        //}
    }
}
