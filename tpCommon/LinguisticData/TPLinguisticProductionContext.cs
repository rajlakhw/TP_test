using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

#nullable disable

namespace LinguisticData
{
    public partial class TPLinguisticProductionContext : DbContext
    {
        public TPLinguisticProductionContext()
        {
        }

        public TPLinguisticProductionContext(DbContextOptions<TPLinguisticProductionContext> options)
            : base(options)
        {
        }

        public virtual DbSet<AuditTrailEvent> AuditTrailEvents { get; set; }
        public virtual DbSet<CmsjurisdictionsToTermKeyRelationship> CmsjurisdictionsToTermKeyRelationships { get; set; }
        public virtual DbSet<Cmslanguage> Cmslanguages { get; set; }
        public virtual DbSet<Cmspublication> Cmspublications { get; set; }
        public virtual DbSet<CmspublicationFamily> CmspublicationFamilies { get; set; }
        public virtual DbSet<CmspublicationRelease> CmspublicationReleases { get; set; }
        public virtual DbSet<CmspublicationsToTermKeyRelationship> CmspublicationsToTermKeyRelationships { get; set; }
        public virtual DbSet<CmspublishingTemplate> CmspublishingTemplates { get; set; }
        public virtual DbSet<ExtendedPropertyDefinition> ExtendedPropertyDefinitions { get; set; }
        public virtual DbSet<ExtendedTermKeyProperty> ExtendedTermKeyProperties { get; set; }
        public virtual DbSet<KavitaTest> KavitaTests { get; set; }
        public virtual DbSet<Kjtemp> Kjtemps { get; set; }
        public virtual DbSet<LinguisticDatabase> LinguisticDatabases { get; set; }
        public virtual DbSet<LinguisticDatabaseAccessPermission> LinguisticDatabaseAccessPermissions { get; set; }
        public virtual DbSet<LinguisticDatabaseListDefinition> LinguisticDatabaseListDefinitions { get; set; }
        public virtual DbSet<LinguisticDatabaseTermEntry> LinguisticDatabaseTermEntries { get; set; }
        public virtual DbSet<LinguisticDatabaseTermKey> LinguisticDatabaseTermKeys { get; set; }
        public virtual DbSet<NetEntTemp> NetEntTemps { get; set; }
        public virtual DbSet<Sheet1> Sheet1s { get; set; }
        public virtual DbSet<TempAtlasCopcoMapping> TempAtlasCopcoMappings { get; set; }
        public virtual DbSet<TempAtlasCopcoTermEntryId> TempAtlasCopcoTermEntryIds { get; set; }
        public virtual DbSet<TempAtlasCopcoTermKeyId> TempAtlasCopcoTermKeyIds { get; set; }
        public virtual DbSet<TempCego> TempCegos { get; set; }
        public virtual DbSet<TempCleanedTranslation> TempCleanedTranslations { get; set; }
        public virtual DbSet<TempCleanedTranslationsConan> TempCleanedTranslationsConans { get; set; }
        public virtual DbSet<TempMalayNetEnt> TempMalayNetEnts { get; set; }
        public virtual DbSet<TempPortuguese> TempPortugueses { get; set; }
        public virtual DbSet<TempPortuguese2> TempPortuguese2s { get; set; }
        public virtual DbSet<TempbostikBalticProductScript> TempbostikBalticProductScripts { get; set; }
        public virtual DbSet<TempbostikBalticTdstechDatum> TempbostikBalticTdstechData { get; set; }
        public virtual DbSet<TempbostikBalticTdstechTable> TempbostikBalticTdstechTables { get; set; }
        public virtual DbSet<TempbostikBalticTdstext> TempbostikBalticTdstexts { get; set; }
        public virtual DbSet<TempbostikImportProductScript> TempbostikImportProductScripts { get; set; }
        public virtual DbSet<TempbostikProductMapping> TempbostikProductMappings { get; set; }
        public virtual DbSet<TempbostikStaticText> TempbostikStaticTexts { get; set; }
        public virtual DbSet<TempbostikTdstechDatum> TempbostikTdstechData { get; set; }
        public virtual DbSet<TempbostikTdstechTable> TempbostikTdstechTables { get; set; }
        public virtual DbSet<TempbostikTdstext> TempbostikTdstexts { get; set; }
        public virtual DbSet<TempnetEntImportTranslation> TempnetEntImportTranslations { get; set; }
        public virtual DbSet<TempnetEntViewAllTermsInRelease> TempnetEntViewAllTermsInReleases { get; set; }
        public virtual DbSet<TermEntriesBeingTranslated> TermEntriesBeingTranslateds { get; set; }
        public virtual DbSet<ViewDataRequiredForComparingChangesBetweenRelease> ViewDataRequiredForComparingChangesBetweenReleases { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseSqlServer("Server=FREDCPDBSSC0016\\SQL1, 50043;Database=TPLinguisticDatabasesProduction;Integrated Security=True;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("Relational:Collation", "Latin1_General_CI_AS");

            modelBuilder.Entity<AuditTrailEvent>(entity =>
            {
                entity.HasComment("Logs information for historical purposes about the creation of terms/strings, products/games, so that customers can view the history of a string over a time period or see who has been doing what in the database");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.ClientIdstringAfter)
                    .HasMaxLength(200)
                    .HasColumnName("ClientIDStringAfter")
                    .HasComment("If set, the customer's ID string for this object, after any change was made - for example a Net Ent GameID or String ID, or a Bostik Product Code.");

                entity.Property(e => e.ClientIdstringBefore)
                    .HasMaxLength(200)
                    .HasColumnName("ClientIDStringBefore")
                    .HasComment("If set, the customer's ID string for this object, before any change was made - for example a Net Ent GameID or String ID, or a Bostik Product Code.");

                entity.Property(e => e.DataObjectId)
                    .HasColumnName("DataObjectID")
                    .HasComment("The ID of the data object which the event relates to");

                entity.Property(e => e.DataObjectTypeId)
                    .HasColumnName("DataObjectTypeID")
                    .HasComment("What kind of object the audit event refers to, e.g. a TermeEntry, a TermKey, a CMSPublicaiton, etc.");

                entity.Property(e => e.EventLoggedDateTime)
                    .HasColumnType("datetime")
                    .HasComment("The date/time when this audit event happened");

                entity.Property(e => e.EventType).HasComment("What kind of audit event this was. Currently a hard-coded number. 0 = created, 1 = deleted, 2 = modified.");

                entity.Property(e => e.ExtranetUserName)
                    .IsRequired()
                    .HasMaxLength(100)
                    .HasComment("The user name for the extranet user who enacted the event");

                entity.Property(e => e.LanguageIanacode)
                    .HasMaxLength(20)
                    .HasColumnName("LanguageIANACode")
                    .HasComment("The language relating to the string (if set) or NULL if not relating to a term entry. Strictly speaking this could be derived from the combination of DataObjectID and DataObjectTypeID, but this is likely to result in more complexity/performance issues with queries");

                entity.Property(e => e.LinguisticDatabaseId)
                    .HasColumnName("LinguisticDatabaseID")
                    .HasComment("The ID of the linguistic database which this event relates to. Strictly speaking this could be derived from the combination of DataObjectID and DataObjectTypeID, but this is likely to result in more complexity/performance issues with queries");

                entity.Property(e => e.MainTextContentAfter).HasComment("If set, main string content for this object, after any change was made - for example the text of a string, or the name of a CMSPublication (game/product)");

                entity.Property(e => e.MainTextContentBefore).HasComment("If set, main string content for this object, before any change was made - for example the text of a string, or the name of a CMSPublication (game/product)");
            });

            modelBuilder.Entity<CmsjurisdictionsToTermKeyRelationship>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("CMSJurisdictionsToTermKeyRelationships");

                entity.HasComment("For Net Ent (and maybe other clients in future??) they want to allow specific strings/terms to be associated with zero or more jurisdictions (i.e. specific laws of a particular regulated country/market). If no jurisdictions are associated, then it is available in all jurisdictions.");

                entity.Property(e => e.JurisdictionId)
                    .HasColumnName("JurisdictionID")
                    .HasComment("The country ID, from the Countries table in the Core database");

                entity.Property(e => e.TermKeyId)
                    .HasColumnName("TermKeyID")
                    .HasComment("The ID (from LinguisticDatabaseTermKeys) of the term key to which the jurisdiction applies");
            });

            modelBuilder.Entity<Cmslanguage>(entity =>
            {
                entity.ToTable("CMSLanguages");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.CreatedDateTime).HasColumnType("datetime");

                entity.Property(e => e.DatabaseId).HasColumnName("DatabaseID");

                entity.Property(e => e.DeletedDateTime).HasColumnType("datetime");

                entity.Property(e => e.LanguageIanacode)
                    .HasMaxLength(20)
                    .HasColumnName("LanguageIANAcode");
            });

            modelBuilder.Entity<Cmspublication>(entity =>
            {
                entity.ToTable("CMSPublications");

                entity.HasComment("Publications created in the CMS, with which individual terms/strings can be associated. In the case of Net Ent (our first CMS client), a publication will correspond to a game.");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.ClientIdstring)
                    .IsRequired()
                    .HasMaxLength(200)
                    .HasColumnName("ClientIDString")
                    .HasComment("An ID used by the client to uniquely reference this publication. In the case of Net Ent, this will be what they consider to be their \"game ID string\".");

                entity.Property(e => e.CmspublicationFamilyId)
                    .HasColumnName("CMSPublicationFamilyID")
                    .HasComment("If set, the ID of the CMSPublicationFamily (from CMSPublicationFamilies) of the family to which this publication belongs");

                entity.Property(e => e.CreatedByExtranetUserName)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.CreatedDateTime).HasColumnType("datetime");

                entity.Property(e => e.CustomTextField1Data).HasMaxLength(200);

                entity.Property(e => e.DeletedByExtranetUserName).HasMaxLength(100);

                entity.Property(e => e.DeletedDateTime).HasColumnType("datetime");

                entity.Property(e => e.LastModifiedByExtranetUserName).HasMaxLength(100);

                entity.Property(e => e.LastModifiedDateTime).HasColumnType("datetime");

                entity.Property(e => e.LinguisticDatabaseId)
                    .HasColumnName("LinguisticDatabaseID")
                    .HasComment("The ID of the glossary (CMS database) with which this publication can be associated");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(200)
                    .HasComment("The name of this publication. In the case of Net Ent, for example, this will be the full name of the game as read by one of their end-users");
            });

            modelBuilder.Entity<CmspublicationFamily>(entity =>
            {
                entity.ToTable("CMSPublicationFamilies");

                entity.HasComment("Optional family of publications (e.g. of products, games) to which a publication can belong (one-to-one), e.g. to capture the fact that Products X and Y belong to the same range of products. In the case of Bostik, this can help with auto-formatting (e.g. colour of text when outputting to templates)");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.ClientIdstring)
                    .HasMaxLength(200)
                    .HasColumnName("ClientIDString")
                    .HasComment("An ID used by the client to uniquely reference this family. Can be NULL.");

                entity.Property(e => e.CreatedByExtranetUserName)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.CreatedDateTime).HasColumnType("datetime");

                entity.Property(e => e.CustomTextField1)
                    .HasMaxLength(200)
                    .HasComment("A custom text field associated with this family. In the case of Bostik, for example, this will be the RGB colour used for outputting text in InDesign templates for this product family");

                entity.Property(e => e.CustomTextField2)
                    .HasMaxLength(200)
                    .HasComment("A custom text field associated with this family. In the case of Bostik, for example, this will be the CMYK colour used for outputting text in InDesign templates for this product family");

                entity.Property(e => e.DeletedByExtranetUserName).HasMaxLength(100);

                entity.Property(e => e.DeletedDateTime).HasColumnType("datetime");

                entity.Property(e => e.LastModifiedByExtranetUserName).HasMaxLength(100);

                entity.Property(e => e.LastModifiedDateTime).HasColumnType("datetime");

                entity.Property(e => e.LinguisticDatabaseId)
                    .HasColumnName("LinguisticDatabaseID")
                    .HasComment("The ID (from LinguisticDatabases) of the database to which this family belongs");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(200)
                    .HasComment("The name of this family. In the case of Bostik, for example, this will be the full name of the product family as read by one of their end-users");
            });

            modelBuilder.Entity<CmspublicationRelease>(entity =>
            {
                entity.ToTable("CMSPublicationReleases");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.ClientBugFixOrEnhancementIdstring)
                    .HasMaxLength(100)
                    .HasColumnName("ClientBugFixOrEnhancementIDString");

                entity.Property(e => e.ClientDescription).IsRequired();

                entity.Property(e => e.ClonedFromReleaseId).HasColumnName("ClonedFromReleaseID");

                entity.Property(e => e.CreatedByExtranetUserName)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.CreatedDateTime).HasColumnType("datetime");

                entity.Property(e => e.DeletedByExtranetUserName).HasMaxLength(100);

                entity.Property(e => e.DeletedDateTime).HasColumnType("datetime");

                entity.Property(e => e.LastModifiedByExtranetUserName).HasMaxLength(100);

                entity.Property(e => e.LastModifiedDateTime).HasColumnType("datetime");

                entity.Property(e => e.LinguisticDatabaseId).HasColumnName("LinguisticDatabaseID");

                entity.Property(e => e.LockedByExtranetUserName).HasMaxLength(100);

                entity.Property(e => e.LockedDateTime).HasColumnType("datetime");

                entity.Property(e => e.ReleaseDateTime).HasColumnType("datetime");
            });

            modelBuilder.Entity<CmspublicationsToTermKeyRelationship>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("CMSPublicationsToTermKeyRelationships");

                entity.HasComment("Expresses the relationships between CMSPublications and LinguisticDatabaseTermKeys: i.e. a term key can relate to multiple CMSPublications (or none). In the example of Net Ent, this means that a particular UI string can refer to 3 different games; or if it refers to no games then it applies to all games");

                entity.HasIndex(e => new { e.TermKeyId, e.CmspublicationId }, "_dta_index_CMSPublicationsToTermKeyRelation_10_981578535__K2_K1_114");

                entity.HasIndex(e => new { e.CmspublicationId, e.TermKeyId }, "_dta_index_CMSPublicationsToTermKeyRelation_8_981578535__K1_K2");

                entity.HasIndex(e => e.CmspublicationId, "_dta_index_CMSPublicationsToTermKeyRelation_c_8_981578535__K1")
                    .IsClustered();

                entity.Property(e => e.CmspublicationId)
                    .HasColumnName("CMSPublicationID")
                    .HasComment("The ID (from CMSPublications) of the publication");

                entity.Property(e => e.TermKeyId)
                    .HasColumnName("TermKeyID")
                    .HasComment("The ID (from LinguisticDatabaseTermKeys) of the term key to which the CMS Publication applies");
            });

            modelBuilder.Entity<CmspublishingTemplate>(entity =>
            {
                entity.ToTable("CMSPublishingTemplates");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.CreatedByExtranetUserName)
                    .IsRequired()
                    .HasMaxLength(100)
                    .HasComment("The extranet user name of the user who created the template");

                entity.Property(e => e.CreatedDateTime)
                    .HasColumnType("datetime")
                    .HasComment("The date and time when this template was added");

                entity.Property(e => e.DeletedByExtranetUserName)
                    .HasMaxLength(100)
                    .HasComment("If set the, name of the extranet user who last modified the template");

                entity.Property(e => e.DeletedDateTime)
                    .HasColumnType("datetime")
                    .HasComment("If set, the date and time when this template was deleted");

                entity.Property(e => e.Description)
                    .HasMaxLength(2000)
                    .HasComment("A human-readable description of this template");

                entity.Property(e => e.LastModifiedByExtranetUserName)
                    .HasMaxLength(100)
                    .HasComment("If set the, name of the extranet user who last modified the template");

                entity.Property(e => e.LastModifiedDateTime)
                    .HasColumnType("datetime")
                    .HasComment("If set, the date and time when this template was last modified");

                entity.Property(e => e.LinguisticDatabaseId)
                    .HasColumnName("LinguisticDatabaseID")
                    .HasComment("The ID of the database (from the LinguisticDatabases table) with which this template is associated");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(500)
                    .HasComment("The name of this template (a human-readable name, not necessarily a file name)");

                entity.Property(e => e.UnderlyingFilePath)
                    .IsRequired()
                    .HasMaxLength(244)
                    .HasComment("The full UNC path to where the template file resides");
            });

            modelBuilder.Entity<ExtendedPropertyDefinition>(entity =>
            {
                entity.HasComment("For some clients, e.g. Atlas Copco, they want a longer list of more flexible extended properties for TermEntries. This defines the available list of property names/IDs for a given linguistic database.");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.ExtendedPropertyName)
                    .IsRequired()
                    .HasMaxLength(200)
                    .HasComment("The name of the extended property as it will appear in a drop-down list for the client");

                entity.Property(e => e.LinguisticDatabaseId)
                    .HasColumnName("LinguisticDatabaseID")
                    .HasComment("The ID mapping to LinguisticDatabases for which this extended property is available");
            });

            modelBuilder.Entity<ExtendedTermKeyProperty>(entity =>
            {
                entity.HasNoKey();

                entity.HasComment("Some clients (e.g. Atlas Copco) have a greater number and more flexible properties to associate with each term key - this table contains the extended properties for each term key");

                entity.Property(e => e.DataType).HasComment("The data is stored in an nvarchar(max) field - PropertyValue - but could actually be handled as an integer as far as the client is concerned. This derives from the fact that Atlas Copco needed the flexibility to choose between a string and an integer. Here 1 = string, 2 = integer and there can be flexibility for other types in future.");

                entity.Property(e => e.ExtendedPropertyDefinitionId)
                    .HasColumnName("ExtendedPropertyDefinitionID")
                    .HasComment("The ID (from ExtendedPropertyDefinitions) of the extended property definition (which contains its name as would be shown to the client)");

                entity.Property(e => e.PropertyValue)
                    .IsRequired()
                    .HasComment("This is the actual value of the extended property, as displayed/published in the content");

                entity.Property(e => e.TermKeyId)
                    .HasColumnName("TermKeyID")
                    .HasComment("The ID from LinguistiDatabaseTermKeys of the term key to which the property is attached");
            });

            modelBuilder.Entity<KavitaTest>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("Kavita_Test");

                entity.Property(e => e.Id).HasColumnName("ID");
            });

            modelBuilder.Entity<Kjtemp>(entity =>
            {
                entity.ToTable("KJTemp");

                entity.Property(e => e.Id)
                    .ValueGeneratedNever()
                    .HasColumnName("ID")
                    .HasComment("The ID for this particular entry (which is a unique combination of the glossary term to which it is attached and the language, custom data and so on)");

                entity.Property(e => e.ActualNumOfCharsInTermText)
                    .HasComputedColumnSql("([dbo].[funcNumberOfCharsInString]([TermText]))", false)
                    .HasComment("The number of characters in TermText (at present this is just using LEN but I will change it in future to use regular expressions to exclude HTML tags, etc.)");

                entity.Property(e => e.CreatedByEmployeeId)
                    .HasColumnName("CreatedByEmployeeID")
                    .HasComment("The translate plus employee who added the data. If this is set to the extranet system user, then it means the data was added via an extranet user, in which case their user name is soterd in CreatedByExtranetUserName");

                entity.Property(e => e.CreatedByExtranetUserName)
                    .HasMaxLength(100)
                    .HasComment("If NULL, the data was added by a translate plus employee. If set then the data was added by an extranet user.");

                entity.Property(e => e.CreatedDateTime)
                    .HasColumnType("datetime")
                    .HasComment("The date and time when this data was added. This relates to the current language data and is independent of when the term as a whole was added. Normally I expect that for the first (source) language data added, this would be identical to the overall term CreatedDateTime, but subsequent languages would differ.");

                entity.Property(e => e.CurrentlyBeingEditedAsOfDateTime)
                    .HasColumnType("datetime")
                    .HasComment("Used for locking concurrent editing of the TermEntry in the extranet - if set, this indicates that someone has started the process of trying to edit it at a specific point in time. Logic is then used to determine if sufficient time has elapsed (e.g. 15 minutes) since the last time this field was updated to see if the user can be regarded as still editing it, or if this lock is then replaced by another user who wants to edit it.");

                entity.Property(e => e.CurrentlyBeingEditedByExtranetUserName)
                    .HasMaxLength(100)
                    .HasComment("Used for locking concurrent editing of the TermEntry in the extranet - if set, this indicates that someone has started the process of trying to edit it.");

                entity.Property(e => e.CustomBitField1Data).HasComment("Optional bit data (True-False/Yes-No/1-0). The LinguisticDatabases table defines what this data relates to for the particular database in question.");

                entity.Property(e => e.CustomImageField1Data).HasComment("Optional image BLOB data. The Glossaries table defines what this data relates to for the particular glossary in question.");

                entity.Property(e => e.CustomListField1Data).HasComment("Optional pick-list data. The LinguisticDatabases table defines what this data relates to for the particular database in question.");

                entity.Property(e => e.CustomListField2Data).HasComment("Optional pick-list data. The LinguisticDatabases table defines what this data relates to for the particular database in question.");

                entity.Property(e => e.CustomListField3Data).HasComment("Optional pick-list data. The LinguisticDatabases table defines what this data relates to for the particular database in question.");

                entity.Property(e => e.CustomListField4Data).HasComment("Optional pick-list data. The LinguisticDatabases table defines what this data relates to for the particular database in question.");

                entity.Property(e => e.CustomTextField1Data).HasComment("Optional free-text data. The LinguisticDatabases table defines what this data relates to for the particular glossary in question.");

                entity.Property(e => e.CustomTextField2Data).HasComment("Optional free-text data. The LinguisticDatabases table defines what this data relates to for the particular glossary in question.");

                entity.Property(e => e.CustomTextField3Data).HasComment("Optional free-text data. The LinguisticDatabases table defines what this data relates to for the particular glossary in question.");

                entity.Property(e => e.CustomTextField4Data).HasComment("Optional free-text data. The LinguisticDatabases table defines what this data relates to for the particular glossary in question.");

                entity.Property(e => e.CustomTextField5Data).HasComment("Optional free-text data. The LinguisticDatabases table defines what this data relates to for the particular glossary in question.");

                entity.Property(e => e.CustomTextField6Data).HasComment("Optional free-text data. The LinguisticDatabases table defines what this data relates to for the particular glossary in question.");

                entity.Property(e => e.DeletedByEmployeeId)
                    .HasColumnName("DeletedByEmployeeID")
                    .HasComment("If set, the translate plus employee who deleted the data. If this is set to the extranet system user, then it means the data was deleted by an extranet user, in which case their user name is stored in DeletedByExtranetUserName");

                entity.Property(e => e.DeletedByExtranetUserName)
                    .HasMaxLength(100)
                    .HasComment("If DeletedDateTime is not NULL but this column is NULL, the data was deleted by a translate plus employee. If set then this is the user name of the extranet user who deleted the data.");

                entity.Property(e => e.DeletedDateTime)
                    .HasColumnType("datetime")
                    .HasComment("The date and time when the data was deleted");

                entity.Property(e => e.LanguageIanacode)
                    .IsRequired()
                    .HasMaxLength(20)
                    .HasColumnName("LanguageIANACode")
                    .HasComment("The IANA code (from the Languages table in the core database) of this term data");

                entity.Property(e => e.LastModifiedByEmployeeId)
                    .HasColumnName("LastModifiedByEmployeeID")
                    .HasComment("The translate plus employee who most recently modified the data. If this is set to the extranet system user, then it means the data was last modified by an extranet user, in which case their user name is soterd in LastModifiedByExtranetUserName");

                entity.Property(e => e.LastModifiedByExtranetUserName)
                    .HasMaxLength(100)
                    .HasComment("If LastModifiedDateTime is not NULL but this column is NULL, the data was last modified by a translate plus employee. If set then this is the user name of the extranet user who last modified the data.");

                entity.Property(e => e.LastModifiedDateTime)
                    .HasColumnType("datetime")
                    .HasComment("The date and time when the data was last modified");

                entity.Property(e => e.LinguisticDatabaseTermId)
                    .HasColumnName("LinguisticDatabaseTermID")
                    .HasComment("The ID of the linguistic database term (from LinguisticDatabaseTermKeys) which this data is associated with");

                entity.Property(e => e.Status).HasComment("The current status of the data. At present 0 = Draft, 1 = Approved, 2 = Being Translated; 3 = Pending Approval; 4 = Ready for translation. But can support future status levels.");

                entity.Property(e => e.TermText)
                    .IsRequired()
                    .HasComment("The actual term itself, i.e. the word/phrase in question");
            });

            modelBuilder.Entity<LinguisticDatabase>(entity =>
            {
                entity.HasComment("Key information about the glossaries available");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.AllowSearchByClientTermKeyId)
                    .HasColumnName("AllowSearchByClientTermKeyID")
                    .HasComment("For use in searching via a user interface: 1/True if this glossary permits users to search by a client-specific term ID (as stored in the TermKeys table), in addition to the usual ability to search by term text. Should be 0/False unless a client has specified any kind of IDs which we import from their system.");

                entity.Property(e => e.ClientTermKeyIdname)
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasColumnName("ClientTermKeyIDName")
                    .HasComment("The human-readable name which the client gives to their own term key IDs, for example \"Net Ent string ID\"");

                entity.Property(e => e.CreatedByEmployeeId)
                    .HasColumnName("CreatedByEmployeeID")
                    .HasComment("The ID of the translate plus employee who created the glossary (on behalf of the client)");

                entity.Property(e => e.CreatedDateTime)
                    .HasColumnType("datetime")
                    .HasComment("The date and time when this glossary was first created");

                entity.Property(e => e.CustomBitField1FalseName)
                    .HasMaxLength(50)
                    .HasComment("If there is a CustomBitField1 then this is the text to be displayed when the value of that field is False/0/No");

                entity.Property(e => e.CustomBitField1Name)
                    .HasMaxLength(50)
                    .HasComment("The name of an optional custom yes-no/true-false/1-0 field; e.g. this might be for something like \"Translatable? Yes/No\". If NULL then the glossary will not display any custom field or field contents to the user.");

                entity.Property(e => e.CustomBitField1Nullname)
                    .HasMaxLength(50)
                    .HasColumnName("CustomBitField1NULLName")
                    .HasComment("If there is a CustomBitField1 then this is the text to be displayed when the value of that field is NULL/not specified (i.e. neither True/1/Yes nor False/0/No)");

                entity.Property(e => e.CustomBitField1TrueName)
                    .HasMaxLength(50)
                    .HasComment("If there is a CustomBitField1 then this is the text to be displayed when the value of that field is True/1/Yes");

                entity.Property(e => e.CustomImageField1Name)
                    .HasMaxLength(50)
                    .HasComment("The name of an optional custom image field; e.g. this might be \"Illustration of part\". If NULL then the glossary will not display any custom field or field contents to the user.");

                entity.Property(e => e.CustomListField1Name)
                    .HasMaxLength(50)
                    .HasComment("The name of an optional custom list-select field; e.g. this might be \"Product Series\" (and then be linked to a list of product series names which the user can pick from). If NULL then the glossary will not display any custom field or field contents to the user.");

                entity.Property(e => e.CustomListField2Name)
                    .HasMaxLength(50)
                    .HasComment("The name of an optional custom list-select field; e.g. this might be \"Product Series\" (and then be linked to a list of product series names which the user can pick from). If NULL then the glossary will not display any custom field or field contents to the user.");

                entity.Property(e => e.CustomListField3Name)
                    .HasMaxLength(50)
                    .HasComment("The name of an optional custom list-select field; e.g. this might be \"Product Series\" (and then be linked to a list of product series names which the user can pick from). If NULL then the glossary will not display any custom field or field contents to the user.");

                entity.Property(e => e.CustomListField4Name)
                    .HasMaxLength(50)
                    .HasComment("The name of an optional custom list-select field; e.g. this might be \"Product Series\" (and then be linked to a list of product series names which the user can pick from). If NULL then the glossary will not display any custom field or field contents to the user.");

                entity.Property(e => e.CustomListField5MultiselectName)
                    .HasMaxLength(50)
                    .HasComment("The name of an optional custom list-select field; e.g. this might be \"Product Series\" (and then be linked to a list of product series names which the user can pick from). If NULL then the glossary will not display any custom field or field contents to the user. ListField3 is multi-select, unlike ListField1 and ListField2, which are designed to be single-select only.\r\n");

                entity.Property(e => e.CustomTextField1Name)
                    .HasMaxLength(50)
                    .HasComment("The name of an optional custom text field; e.g. this might be \"Definition\" or \"Notes\". If NULL then the glossary will not display any custom field or field contents to the user.");

                entity.Property(e => e.CustomTextField2Name)
                    .HasMaxLength(50)
                    .HasComment("The name of an optional custom text field; e.g. this might be \"Definition\" or \"Notes\". If NULL then the glossary will not display any custom field or field contents to the user.");

                entity.Property(e => e.CustomTextField3Name)
                    .HasMaxLength(50)
                    .HasComment("The name of an optional custom text field; e.g. this might be \"Definition\" or \"Notes\". If NULL then the glossary will not display any custom field or field contents to the user.");

                entity.Property(e => e.CustomTextField4Name)
                    .HasMaxLength(50)
                    .HasComment("The name of an optional custom text field; e.g. this might be \"Definition\" or \"Notes\". If NULL then the glossary will not display any custom field or field contents to the user.");

                entity.Property(e => e.CustomTextField5Name)
                    .HasMaxLength(50)
                    .HasComment("The name of an optional custom text field; e.g. this might be \"Definition\" or \"Notes\". If NULL then the glossary will not display any custom field or field contents to the user.");

                entity.Property(e => e.CustomTextField6Name)
                    .HasMaxLength(50)
                    .HasComment("The name of an optional custom text field; e.g. this might be \"Definition\" or \"Notes\". If NULL then the glossary will not display any custom field or field contents to the user.");

                entity.Property(e => e.DefaultSourceLangIanacode)
                    .HasMaxLength(20)
                    .HasColumnName("DefaultSourceLangIANACode")
                    .HasComment("If set, used to populate the default language shown in any drop-down lists");

                entity.Property(e => e.DeletedByEmployeeId)
                    .HasColumnName("DeletedByEmployeeID")
                    .HasComment("The ID of the translate plus employee who deleted the glossary");

                entity.Property(e => e.DeletedDateTime)
                    .HasColumnType("datetime")
                    .HasComment("The date and time this glossary was deleted. If NULL then it is still live/in use. If set then none of the attached glossary terms will be accessible, whether or not those terms themselves are marked as Deleted.");

                entity.Property(e => e.Description).HasComment("An optional description of the glossary (anything in this column would be visible to clients and translators)");

                entity.Property(e => e.EmailAddressesToNotifyOnAddEditOrDeleteTermEntries).HasComment("If set, any comma-separated e-mail addresses in this field will be notified automatically every time anyone adds, edits or deletes a term entry");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(100)
                    .HasComment("The name of this glossary as it will be displayed to clients, translators and project managers");

                entity.Property(e => e.ShowAdvancedSearchByDefault).HasComment("If True, in the Extranet interface users are by default taken to a more complex advanced search page compared to the usual one.");

                entity.Property(e => e.UsesCmspublicationRelease)
                    .HasColumnName("UsesCMSPublicationRelease")
                    .HasComment("True if the database makes use of the CMSPublicationRelease functionality (initially this will only be for Net Ent); controls whether links and values relating to this functionality are displayed in the extranet UI, for example");
            });

            modelBuilder.Entity<LinguisticDatabaseAccessPermission>(entity =>
            {
                entity.HasNoKey();

                entity.HasComment("Defines which users (clients and suppliers) have what level of access to which glossaries. A user will only have access to a glossary if they have an entry in this table - if an entry is removed then they no longer have any access to the glossary.");

                entity.HasIndex(e => new { e.DataObjectTypeId, e.DataObjectId, e.LinguisticDatabaseId }, "_dta_index_LinguisticDatabaseAccessPermissi_c_10_2137058649__K2_K3_K1")
                    .IsClustered();

                entity.Property(e => e.AccessLevel).HasComment("Determines the permissions granted to the relevant data object. At present hard-coded: 0 = read-only; 1 = full control; other levels to follow in future");

                entity.Property(e => e.DataObjectId)
                    .HasColumnName("DataObjectID")
                    .HasComment("The ID of the actual object (contact, linguistic supplier, etc.) who is being granted access");

                entity.Property(e => e.DataObjectTypeId)
                    .HasColumnName("DataObjectTypeID")
                    .HasComment("The ID of the type of object (contact, linguistic supplier, etc.) who is being granted access");

                entity.Property(e => e.LinguisticDatabaseId)
                    .HasColumnName("LinguisticDatabaseID")
                    .HasComment("The ID (from LinguisticDatabases) of the linguistic database for which access is being determined");
            });

            modelBuilder.Entity<LinguisticDatabaseListDefinition>(entity =>
            {
                entity.HasNoKey();

                entity.HasComment("Defines the options available to the various glossaries for custom pick-list data");

                entity.HasIndex(e => new { e.LinguisticDatabaseId, e.ListItemIndex, e.ListNumber }, "_dta_index_LinguisticDatabaseListDefinition_c_10_1893581784__K1_K3_K2")
                    .IsClustered();

                entity.Property(e => e.LinguisticDatabaseId)
                    .HasColumnName("LinguisticDatabaseID")
                    .HasComment("The ID (from LinguisticDatabases) of the linguistic database with which the picklist option will be associated");

                entity.Property(e => e.ListItemIndex).HasComment("The position within the list that this item represents. For example, if this is 3 then the label/option associated with it will be displayed in between those with indexes 2 and 4.");

                entity.Property(e => e.ListItemName)
                    .IsRequired()
                    .HasMaxLength(100)
                    .HasComment("The text which is displayed to the user for this item, e.g. it could be \"Desktops\", \"Servers\", etc. if the usage of this list is \"hardware product ranges\" for a particular client category");

                entity.Property(e => e.ListNumber).HasComment("Which field number the data applies to. At present only 2 pick lists are permitted, so this will be 1 or 2");
            });

            modelBuilder.Entity<LinguisticDatabaseTermEntry>(entity =>
            {
                entity.HasComment("Contains the language-specific data for each glossary term, linked to the ID of the GlossaryTermKeys table");

                entity.HasIndex(e => new { e.DeletedDateTime, e.LanguageIanacode, e.CustomListField1Data, e.LinguisticDatabaseTermId }, "_dta_index_LinguisticDatabaseTermEntries_10_1781020455__K25_K3_K13_K2_1_4_6_7");

                entity.HasIndex(e => new { e.DeletedDateTime, e.LanguageIanacode, e.LinguisticDatabaseTermId, e.CustomListField1Data, e.Id }, "_dta_index_LinguisticDatabaseTermEntries_10_1781020455__K25_K3_K2_K13_K1_4_6_7");

                entity.HasIndex(e => new { e.DeletedDateTime, e.LanguageIanacode, e.LinguisticDatabaseTermId, e.CustomListField1Data, e.Id }, "_dta_index_LinguisticDatabaseTermEntries_10_1781020455__K25_K3_K2_K13_K1_4_6_7_14");

                entity.HasIndex(e => new { e.DeletedDateTime, e.Status, e.LanguageIanacode, e.LinguisticDatabaseTermId, e.Id, e.CreatedDateTime }, "_dta_index_LinguisticDatabaseTermEntries_10_1781020455__K25_K6_K3_K2_K1_K19_4_7_10_11_13_14");

                entity.HasIndex(e => new { e.DeletedDateTime, e.LanguageIanacode, e.CustomListField1Data, e.LinguisticDatabaseTermId }, "_dta_index_LinguisticDatabaseTermEntries_8_1779641533__K22_K3_K10_K2_1_4");

                entity.HasIndex(e => new { e.DeletedDateTime, e.LanguageIanacode, e.LinguisticDatabaseTermId, e.Id, e.CustomListField1Data }, "_dta_index_LinguisticDatabaseTermEntries_8_1779641533__K22_K3_K2_K1_K10_4");

                entity.HasIndex(e => new { e.DeletedDateTime, e.Status, e.LanguageIanacode, e.Id, e.LinguisticDatabaseTermId, e.CreatedDateTime }, "_dta_index_LinguisticDatabaseTermEntries_8_837578022__K20_K6_K3_K1_K2_K14");

                entity.HasIndex(e => new { e.DeletedDateTime, e.Status, e.LanguageIanacode, e.LinguisticDatabaseTermId }, "_dta_index_LinguisticDatabaseTermEntries_8_837578022__K20_K6_K3_K2");

                entity.HasIndex(e => new { e.LinguisticDatabaseTermId, e.DeletedDateTime, e.LanguageIanacode, e.Status }, "_dta_index_LinguisticDatabaseTermEntries_8_837578022__K2_K20_K3_K6_1_4");

                entity.HasIndex(e => new { e.LanguageIanacode, e.DeletedDateTime, e.LinguisticDatabaseTermId }, "_dta_index_LinguisticDatabaseTermEntries_8_837578022__K3_K20_K2");

                entity.HasIndex(e => new { e.LanguageIanacode, e.DeletedDateTime, e.LinguisticDatabaseTermId }, "_dta_index_LinguisticDatabaseTermEntries_8_837578022__K3_K20_K2_1_4");

                entity.HasIndex(e => new { e.Status, e.DeletedDateTime, e.LanguageIanacode, e.Id, e.LinguisticDatabaseTermId }, "_dta_index_LinguisticDatabaseTermEntries_8_837578022__K6_K20_K3_K1_K2_4");

                entity.Property(e => e.Id)
                    .HasColumnName("ID")
                    .HasComment("The ID for this particular entry (which is a unique combination of the glossary term to which it is attached and the language, custom data and so on)");

                entity.Property(e => e.ActualNumOfCharsInTermText)
                    .HasComputedColumnSql("([dbo].[funcNumberOfCharsInString]([TermText]))", false)
                    .HasComment("The number of characters in TermText (at present this is just using LEN but I will change it in future to use regular expressions to exclude HTML tags, etc.)");

                entity.Property(e => e.CreatedByEmployeeId)
                    .HasColumnName("CreatedByEmployeeID")
                    .HasComment("The translate plus employee who added the data. If this is set to the extranet system user, then it means the data was added via an extranet user, in which case their user name is soterd in CreatedByExtranetUserName");

                entity.Property(e => e.CreatedByExtranetUserName)
                    .HasMaxLength(100)
                    .HasComment("If NULL, the data was added by a translate plus employee. If set then the data was added by an extranet user.");

                entity.Property(e => e.CreatedDateTime)
                    .HasColumnType("datetime")
                    .HasComment("The date and time when this data was added. This relates to the current language data and is independent of when the term as a whole was added. Normally I expect that for the first (source) language data added, this would be identical to the overall term CreatedDateTime, but subsequent languages would differ.");

                entity.Property(e => e.CurrentlyBeingEditedAsOfDateTime)
                    .HasColumnType("datetime")
                    .HasComment("Used for locking concurrent editing of the TermEntry in the extranet - if set, this indicates that someone has started the process of trying to edit it at a specific point in time. Logic is then used to determine if sufficient time has elapsed (e.g. 15 minutes) since the last time this field was updated to see if the user can be regarded as still editing it, or if this lock is then replaced by another user who wants to edit it.");

                entity.Property(e => e.CurrentlyBeingEditedByExtranetUserName)
                    .HasMaxLength(100)
                    .HasComment("Used for locking concurrent editing of the TermEntry in the extranet - if set, this indicates that someone has started the process of trying to edit it.");

                entity.Property(e => e.CustomBitField1Data).HasComment("Optional bit data (True-False/Yes-No/1-0). The LinguisticDatabases table defines what this data relates to for the particular database in question.");

                entity.Property(e => e.CustomImageField1Data).HasComment("Optional image BLOB data. The Glossaries table defines what this data relates to for the particular glossary in question.");

                entity.Property(e => e.CustomListField1Data).HasComment("Optional pick-list data. The LinguisticDatabases table defines what this data relates to for the particular database in question.");

                entity.Property(e => e.CustomListField2Data).HasComment("Optional pick-list data. The LinguisticDatabases table defines what this data relates to for the particular database in question.");

                entity.Property(e => e.CustomListField3Data).HasComment("Optional pick-list data. The LinguisticDatabases table defines what this data relates to for the particular database in question.");

                entity.Property(e => e.CustomListField4Data).HasComment("Optional pick-list data. The LinguisticDatabases table defines what this data relates to for the particular database in question.");

                entity.Property(e => e.CustomTextField1Data).HasComment("Optional free-text data. The LinguisticDatabases table defines what this data relates to for the particular glossary in question.");

                entity.Property(e => e.CustomTextField2Data).HasComment("Optional free-text data. The LinguisticDatabases table defines what this data relates to for the particular glossary in question.");

                entity.Property(e => e.CustomTextField3Data).HasComment("Optional free-text data. The LinguisticDatabases table defines what this data relates to for the particular glossary in question.");

                entity.Property(e => e.CustomTextField4Data).HasComment("Optional free-text data. The LinguisticDatabases table defines what this data relates to for the particular glossary in question.");

                entity.Property(e => e.CustomTextField5Data).HasComment("Optional free-text data. The LinguisticDatabases table defines what this data relates to for the particular glossary in question.");

                entity.Property(e => e.CustomTextField6Data).HasComment("Optional free-text data. The LinguisticDatabases table defines what this data relates to for the particular glossary in question.");

                entity.Property(e => e.DeletedByEmployeeId)
                    .HasColumnName("DeletedByEmployeeID")
                    .HasComment("If set, the translate plus employee who deleted the data. If this is set to the extranet system user, then it means the data was deleted by an extranet user, in which case their user name is stored in DeletedByExtranetUserName");

                entity.Property(e => e.DeletedByExtranetUserName)
                    .HasMaxLength(100)
                    .HasComment("If DeletedDateTime is not NULL but this column is NULL, the data was deleted by a translate plus employee. If set then this is the user name of the extranet user who deleted the data.");

                entity.Property(e => e.DeletedDateTime)
                    .HasColumnType("datetime")
                    .HasComment("The date and time when the data was deleted");

                entity.Property(e => e.LanguageIanacode)
                    .IsRequired()
                    .HasMaxLength(20)
                    .HasColumnName("LanguageIANACode")
                    .HasComment("The IANA code (from the Languages table in the core database) of this term data");

                entity.Property(e => e.LastModifiedByEmployeeId)
                    .HasColumnName("LastModifiedByEmployeeID")
                    .HasComment("The translate plus employee who most recently modified the data. If this is set to the extranet system user, then it means the data was last modified by an extranet user, in which case their user name is soterd in LastModifiedByExtranetUserName");

                entity.Property(e => e.LastModifiedByExtranetUserName)
                    .HasMaxLength(100)
                    .HasComment("If LastModifiedDateTime is not NULL but this column is NULL, the data was last modified by a translate plus employee. If set then this is the user name of the extranet user who last modified the data.");

                entity.Property(e => e.LastModifiedDateTime)
                    .HasColumnType("datetime")
                    .HasComment("The date and time when the data was last modified");

                entity.Property(e => e.LinguisticDatabaseTermId)
                    .HasColumnName("LinguisticDatabaseTermID")
                    .HasComment("The ID of the linguistic database term (from LinguisticDatabaseTermKeys) which this data is associated with");

                entity.Property(e => e.Status).HasComment("The current status of the data. At present 0 = Draft, 1 = Approved, 2 = Being Translated; 3 = Pending Approval; 4 = Ready for translation. But can support future status levels.");

                entity.Property(e => e.TermText).HasComment("The actual term itself, i.e. the word/phrase in question");
            });

            modelBuilder.Entity<LinguisticDatabaseTermKey>(entity =>
            {
                entity.HasComment("Each row in this table represents a key/lookup to a (theoretically infinite) number of language representations of the term. No actual term data is stored in this table, only linkage between language terms and which glossary and which other languages they \"belong\" to");

                entity.HasIndex(e => new { e.LinguisticDatabaseId, e.CmspublicationReleaseId, e.DeletedDateTime, e.Id }, "_dta_index_LinguisticDatabaseTermKeys_10_665107496__K2_K4_K8_K1_3");

                entity.HasIndex(e => new { e.Id, e.DeletedDateTime, e.CmspublicationReleaseId }, "_dta_index_LinguisticDatabaseTermKeys_8_757577737__K1_K8_K4");

                entity.HasIndex(e => new { e.LinguisticDatabaseId, e.ClientTermKeyId, e.Id, e.CmspublicationReleaseId }, "_dta_index_LinguisticDatabaseTermKeys_8_757577737__K2_K3_K1_K4");

                entity.HasIndex(e => new { e.LinguisticDatabaseId, e.ClientTermKeyId, e.DeletedDateTime }, "_dta_index_LinguisticDatabaseTermKeys_8_757577737__K2_K3_K8_1");

                entity.HasIndex(e => new { e.LinguisticDatabaseId, e.CmspublicationReleaseId, e.ClientTermKeyId }, "_dta_index_LinguisticDatabaseTermKeys_8_757577737__K2_K4_K3");

                entity.HasIndex(e => new { e.LinguisticDatabaseId, e.DeletedDateTime, e.Id }, "_dta_index_LinguisticDatabaseTermKeys_8_757577737__K2_K8_K1");

                entity.HasIndex(e => new { e.LinguisticDatabaseId, e.DeletedDateTime, e.Id, e.CmspublicationReleaseId }, "_dta_index_LinguisticDatabaseTermKeys_8_757577737__K2_K8_K1_K4_3");

                entity.HasIndex(e => new { e.DeletedDateTime, e.LinguisticDatabaseId, e.CmspublicationReleaseId, e.Id }, "_dta_index_LinguisticDatabaseTermKeys_8_757577737__K8_K2_K4_K1");

                entity.HasIndex(e => new { e.DeletedDateTime, e.CmspublicationReleaseId, e.ClientTermKeyId, e.Id, e.LinguisticDatabaseId }, "_dta_index_LinguisticDatabaseTermKeys_8_757577737__K8_K4_K3_K1_K2");

                entity.Property(e => e.Id)
                    .HasColumnName("ID")
                    .HasComment("The ID of this term. The ID is unique across all glossaries (in other words, there can only be one term with ID 666, rather than an ID 666 within each glossary)");

                entity.Property(e => e.ClientTermKeyId)
                    .HasMaxLength(250)
                    .HasColumnName("ClientTermKeyID")
                    .HasComment("This can be used for storing any IDs provided by the client's system which may be required for synchronising/importing/updating content from their own glossary/database with entries in ours, so that we can map our internal IDs with their IDs. This is an nvarchar because different clients' IDs could be strings, numbers, etc.");

                entity.Property(e => e.CmspublicationReleaseId)
                    .HasColumnName("CMSPublicationReleaseID")
                    .HasComment("If set, this specifies the ID (from CMSPublicationReleases) of the release to which this term key (and hence its associated term entries) applies. In the case of Net Ent, this allows us to say, e.g. \"this string, including its English and translations, belongs to release 11.2\"");

                entity.Property(e => e.CreatedByEmployeeId)
                    .HasColumnName("CreatedByEmployeeID")
                    .HasComment("The translate plus employee who added the term. If this is set to the extranet system user, then it means the term was added via an extranet user, in which case their user name is soterd in CreatedByExtranetUserName");

                entity.Property(e => e.CreatedByExtranetUserName)
                    .HasMaxLength(100)
                    .HasComment("If NULL, the term was added by a translate plus employee. If set then the term was added by an extranet user.");

                entity.Property(e => e.CreatedDateTime)
                    .HasColumnType("datetime")
                    .HasComment("The date and time the term was added. The individual language variants/data associted with the term can have different creation dates.");

                entity.Property(e => e.DeletedByEmployeeId)
                    .HasColumnName("DeletedByEmployeeID")
                    .HasComment("If set, the translate plus employee who deleted the entire term. If this is set to the extranet system user, then it means the term was deleted by an extranet user, in which case their user name is stored in DeletedByExtranetUserName");

                entity.Property(e => e.DeletedByExtranetUserName)
                    .HasMaxLength(100)
                    .HasComment("If DeletedDateTime is not NULL but this column is NULL, the term was deleted by a translate plus employee. If set then this is the user name of the extranet user who deleted the entire term.");

                entity.Property(e => e.DeletedDateTime)
                    .HasColumnType("datetime")
                    .HasComment("If set, the date and time the entire term was deleted; if NULL, the term is still live/in use");

                entity.Property(e => e.LinguisticDatabaseId)
                    .HasColumnName("LinguisticDatabaseID")
                    .HasComment("Which linguistic database (from the LinguisticDatabases table) this term belongs to");
            });

            modelBuilder.Entity<NetEntTemp>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("NetEntTemp");

                entity.Property(e => e.CreatedByEmployeeId).HasColumnName("CreatedByEmployeeID");

                entity.Property(e => e.CreatedByExtranetUserName).HasMaxLength(100);

                entity.Property(e => e.CreatedDateTime).HasColumnType("datetime");

                entity.Property(e => e.CurrentlyBeingEditedAsOfDateTime).HasColumnType("datetime");

                entity.Property(e => e.CurrentlyBeingEditedByExtranetUserName).HasMaxLength(100);

                entity.Property(e => e.DeletedByEmployeeId).HasColumnName("DeletedByEmployeeID");

                entity.Property(e => e.DeletedByExtranetUserName).HasMaxLength(100);

                entity.Property(e => e.DeletedDateTime).HasColumnType("datetime");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.LanguageIanacode)
                    .HasMaxLength(20)
                    .HasColumnName("LanguageIANACode");

                entity.Property(e => e.LastModifiedByEmployeeId).HasColumnName("LastModifiedByEmployeeID");

                entity.Property(e => e.LastModifiedByExtranetUserName).HasMaxLength(100);

                entity.Property(e => e.LastModifiedDateTime).HasColumnType("datetime");

                entity.Property(e => e.LinguisticDatabaseTermId).HasColumnName("LinguisticDatabaseTermID");
            });

            modelBuilder.Entity<Sheet1>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("Sheet1$");

                entity.Property(e => e.ClientTermKeyId)
                    .HasMaxLength(255)
                    .HasColumnName("ClientTermKeyID");

                entity.Property(e => e.CreatedByEmployeeId).HasColumnName("CreatedByEmployeeID");

                entity.Property(e => e.CreatedByExtranetUserName).HasMaxLength(255);

                entity.Property(e => e.CreatedDateTime).HasColumnType("datetime");

                entity.Property(e => e.CustomTextField1Data).HasMaxLength(255);

                entity.Property(e => e.CustomTextField2Data).HasMaxLength(255);

                entity.Property(e => e.CustomTextField3Data).HasMaxLength(255);

                entity.Property(e => e.CustomTextField4Data).HasMaxLength(255);

                entity.Property(e => e.CustomTextField5Data).HasMaxLength(255);

                entity.Property(e => e.CustomTextField6Data).HasMaxLength(255);

                entity.Property(e => e.LanguageIanacode)
                    .HasMaxLength(255)
                    .HasColumnName("LanguageIANACode");

                entity.Property(e => e.LinguisticDatabaseTermId).HasColumnName("LinguisticDatabaseTermID");

                entity.Property(e => e.TermText).HasMaxLength(255);
            });

            modelBuilder.Entity<TempAtlasCopcoMapping>(entity =>
            {
                entity.HasNoKey();

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.TermKeyIdtoMapTo).HasColumnName("TermKeyIDToMapTo");
            });

            modelBuilder.Entity<TempAtlasCopcoTermEntryId>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("TempAtlasCopcoTermEntryIDs");

                entity.Property(e => e.AtlasCopcoOriginalId).HasColumnName("AtlasCopcoOriginalID");

                entity.Property(e => e.TptermEntryId).HasColumnName("TPTermEntryID");
            });

            modelBuilder.Entity<TempAtlasCopcoTermKeyId>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("TempAtlasCopcoTermKeyIDs");

                entity.Property(e => e.AtlasCopcoOriginalId).HasColumnName("AtlasCopcoOriginalID");

                entity.Property(e => e.TptermKeyId).HasColumnName("TPTermKeyID");
            });

            modelBuilder.Entity<TempCego>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("Temp_CEGO");

                entity.Property(e => e.CegoStringId)
                    .HasMaxLength(255)
                    .HasColumnName("Cego String Id");

                entity.Property(e => e.CmsPublicationId).HasColumnName("CMS Publication ID");

                entity.Property(e => e.Danish).HasMaxLength(1000);

                entity.Property(e => e.English).HasMaxLength(1000);

                entity.Property(e => e.Norwegian).HasMaxLength(1000);

                entity.Property(e => e.TermKeyId).HasColumnName("TermKeyID");
            });

            modelBuilder.Entity<TempCleanedTranslation>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("Temp_Cleaned_Translations");

                entity.Property(e => e.TptranslationTermId).HasColumnName("TPTranslationTermID");
            });

            modelBuilder.Entity<TempCleanedTranslationsConan>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("Temp_Cleaned_Translations Conan");

                entity.Property(e => e.TermId).HasColumnName("Term ID");
            });

            modelBuilder.Entity<TempMalayNetEnt>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("tempMalayNetEnt");

                entity.Property(e => e.CustomerSpecificId).HasColumnName("CustomerSpecificID");

                entity.Property(e => e.TermId).HasColumnName("TermID");
            });

            modelBuilder.Entity<TempPortuguese>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("Temp_Portuguese");

                entity.Property(e => e.TptranslationTermId).HasColumnName("TPTranslationTermID");
            });

            modelBuilder.Entity<TempPortuguese2>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("Temp_Portuguese_2");

                entity.Property(e => e.TptranslationTermId).HasColumnName("TPTranslationTermID");
            });

            modelBuilder.Entity<TempbostikBalticProductScript>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("TEMPBostikBalticProductScript");

                entity.Property(e => e.LanguageCode).HasMaxLength(255);

                entity.Property(e => e.ProductNo).HasMaxLength(255);
            });

            modelBuilder.Entity<TempbostikBalticTdstechDatum>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("TEMPBostikBalticTDSTechData");

                entity.Property(e => e.Tdhead).HasColumnName("TDHEAD");

                entity.Property(e => e.Tdhseq).HasColumnName("TDHSEQ");

                entity.Property(e => e.Tditcl)
                    .HasMaxLength(255)
                    .HasColumnName("TDITCL");

                entity.Property(e => e.Tdlncd)
                    .HasMaxLength(255)
                    .HasColumnName("TDLNCD");

                entity.Property(e => e.Tdprop).HasColumnName("TDPROP");

                entity.Property(e => e.Tdseqn).HasColumnName("TDSEQN");

                entity.Property(e => e.Tdvalu).HasColumnName("TDVALU");
            });

            modelBuilder.Entity<TempbostikBalticTdstechTable>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("TEMPBostikBalticTDSTechTables");

                entity.Property(e => e.Tdcolv).HasColumnName("TDCOLV");

                entity.Property(e => e.Tdcseq).HasColumnName("TDCSEQ");

                entity.Property(e => e.Tdcszp).HasColumnName("TDCSZP");

                entity.Property(e => e.Tditcl)
                    .HasMaxLength(255)
                    .HasColumnName("TDITCL");

                entity.Property(e => e.Tdlncd)
                    .HasMaxLength(255)
                    .HasColumnName("TDLNCD");

                entity.Property(e => e.Tdrowv).HasColumnName("TDROWV");

                entity.Property(e => e.Tdrseq).HasColumnName("TDRSEQ");

                entity.Property(e => e.Tdtabd).HasColumnName("TDTABD");

                entity.Property(e => e.Tdtseq).HasColumnName("TDTSEQ");
            });

            modelBuilder.Entity<TempbostikBalticTdstext>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("TEMPBostikBalticTDSTexts");

                entity.Property(e => e.Tditcl)
                    .HasMaxLength(255)
                    .HasColumnName("TDITCL");

                entity.Property(e => e.Tdlncd)
                    .HasMaxLength(255)
                    .HasColumnName("TDLNCD");

                entity.Property(e => e.Tdtext).HasColumnName("TDText");

                entity.Property(e => e.Tdtype)
                    .HasMaxLength(255)
                    .HasColumnName("TDTYPE");
            });

            modelBuilder.Entity<TempbostikImportProductScript>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("TEMPBostikImportProductScript");

                entity.Property(e => e.LanguageCode).HasMaxLength(255);

                entity.Property(e => e.ProductNo).HasMaxLength(255);
            });

            modelBuilder.Entity<TempbostikProductMapping>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("TEMPBostikProductMappings");

                entity.Property(e => e._1)
                    .HasMaxLength(255)
                    .HasColumnName("1");

                entity.Property(e => e._10)
                    .HasMaxLength(255)
                    .HasColumnName("10");

                entity.Property(e => e._11)
                    .HasMaxLength(255)
                    .HasColumnName("11");

                entity.Property(e => e._13)
                    .HasMaxLength(255)
                    .HasColumnName("13");

                entity.Property(e => e._2)
                    .HasMaxLength(255)
                    .HasColumnName("2");

                entity.Property(e => e._3)
                    .HasMaxLength(255)
                    .HasColumnName("3");

                entity.Property(e => e._4)
                    .HasMaxLength(255)
                    .HasColumnName("4");

                entity.Property(e => e._5)
                    .HasMaxLength(255)
                    .HasColumnName("5");

                entity.Property(e => e._6)
                    .HasMaxLength(255)
                    .HasColumnName("6");

                entity.Property(e => e._7)
                    .HasMaxLength(255)
                    .HasColumnName("7");

                entity.Property(e => e._8)
                    .HasMaxLength(255)
                    .HasColumnName("8");

                entity.Property(e => e._9)
                    .HasMaxLength(255)
                    .HasColumnName("9");
            });

            modelBuilder.Entity<TempbostikStaticText>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("TEMPBostikStaticText");

                entity.Property(e => e.Dk).HasColumnName("DK");

                entity.Property(e => e.Ee).HasColumnName("EE");

                entity.Property(e => e.Fi).HasColumnName("FI");

                entity.Property(e => e.Gb).HasColumnName("GB");

                entity.Property(e => e.IPlusStringId)
                    .HasMaxLength(255)
                    .HasColumnName("i plus string ID");

                entity.Property(e => e.Lt).HasColumnName("LT");

                entity.Property(e => e.Lv).HasColumnName("LV");

                entity.Property(e => e.No).HasColumnName("NO");

                entity.Property(e => e.Se).HasColumnName("SE");
            });

            modelBuilder.Entity<TempbostikTdstechDatum>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("TEMPBostikTDSTechData");

                entity.Property(e => e.Tdhead).HasColumnName("TDHEAD");

                entity.Property(e => e.Tdhseq).HasColumnName("TDHSEQ");

                entity.Property(e => e.Tditcl)
                    .HasMaxLength(255)
                    .HasColumnName("TDITCL");

                entity.Property(e => e.Tdlncd)
                    .HasMaxLength(255)
                    .HasColumnName("TDLNCD");

                entity.Property(e => e.Tdprop).HasColumnName("TDPROP");

                entity.Property(e => e.Tdseqn).HasColumnName("TDSEQN");

                entity.Property(e => e.Tdvalu).HasColumnName("TDVALU");
            });

            modelBuilder.Entity<TempbostikTdstechTable>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("TEMPBostikTDSTechTables");

                entity.Property(e => e.Tdcolv).HasColumnName("TDCOLV");

                entity.Property(e => e.Tdcseq).HasColumnName("TDCSEQ");

                entity.Property(e => e.Tdcszp).HasColumnName("TDCSZP");

                entity.Property(e => e.Tditcl)
                    .HasMaxLength(255)
                    .HasColumnName("TDITCL");

                entity.Property(e => e.Tdlncd)
                    .HasMaxLength(255)
                    .HasColumnName("TDLNCD");

                entity.Property(e => e.Tdrowv).HasColumnName("TDROWV");

                entity.Property(e => e.Tdrseq).HasColumnName("TDRSEQ");

                entity.Property(e => e.Tdtabd).HasColumnName("TDTABD");

                entity.Property(e => e.Tdtseq).HasColumnName("TDTSEQ");
            });

            modelBuilder.Entity<TempbostikTdstext>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("TEMPBostikTDSTexts");

                entity.Property(e => e.Tditcl).HasColumnName("TDITCL");

                entity.Property(e => e.Tdlncd).HasColumnName("TDLNCD");

                entity.Property(e => e.Tdtext).HasColumnName("TDTEXT");

                entity.Property(e => e.Tdtype).HasColumnName("TDTYPE");
            });

            modelBuilder.Entity<TempnetEntImportTranslation>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("TEMPNetEntImportTranslations");

                entity.Property(e => e.ClientTermKeyId)
                    .HasMaxLength(255)
                    .HasColumnName("ClientTermKeyID");

                entity.Property(e => e.TptranslationTermId).HasColumnName("TPTranslationTermID");

                entity.Property(e => e.TranslationLanguage).HasMaxLength(10);
            });

            modelBuilder.Entity<TempnetEntViewAllTermsInRelease>(entity =>
            {
                entity.HasNoKey();

                entity.ToView("TEMPNetEntViewAllTermsInRelease");

                entity.Property(e => e.ClientTermKeyId)
                    .HasMaxLength(50)
                    .HasColumnName("ClientTermKeyID");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.LanguageIanacode)
                    .IsRequired()
                    .HasMaxLength(20)
                    .HasColumnName("LanguageIANACode");
            });

            modelBuilder.Entity<TermEntriesBeingTranslated>(entity =>
            {
                entity.ToTable("TermEntriesBeingTranslated");

                entity.HasComment("Captures information on term entries which are currently undergoing translation. Once translation is totally complete, records remain in this table for historical/audit reasons so that we can subsequently check which terms were translated in which languages as part of which order.");

                entity.HasIndex(e => new { e.AttachedToJobItemId, e.TranslationStatus }, "_dta_index_TermEntriesBeingTranslated_8_2101582525__K2_K4_3");

                entity.HasIndex(e => new { e.AttachedToJobItemId, e.TranslationStatus, e.TermEntryId, e.Id }, "_dta_index_TermEntriesBeingTranslated_8_2101582525__K2_K4_K3_K1_5_9");

                entity.Property(e => e.Id)
                    .HasColumnName("ID")
                    .HasComment("Unique ID for this record, as distinct from the term entry ID or the job order ID it is attached to");

                entity.Property(e => e.AttachedToJobItemId)
                    .HasColumnName("AttachedToJobItemID")
                    .HasComment("From here (related to the JobItems table in the Core database) we can trace which translator it was assigned to, deadlines, etc., and thus also which overall job order it belongs to");

                entity.Property(e => e.FinalDeliveryToLinguisticDatabaseByEmployeeId)
                    .HasColumnName("FinalDeliveryToLinguisticDatabaseByEmployeeID")
                    .HasComment("If set, this was the employee who \"delivered\" the translations back to the LinguisticDatabase proper, for the customer to then be able to access the new/updated translations. From this point onwards the job is \"finished\" and no further changes will be made to this row/the translations as part of the translation cycle (though of course the terms themselves can subsequently be modified in the Linguistic Database itself).");

                entity.Property(e => e.FinalDeliveryToLinguisticDatabaseDateTime)
                    .HasColumnType("datetime")
                    .HasComment("If set, this was when we \"delivered\" the translations back to the LinguisticDatabase proper, for the customer to then be able to access the new/updated translations. From this point onwards the job is \"finished\" and no further changes will be made to this row/the translations as part of the translation cycle (though of course the terms themselves can subsequently be modified in the Linguistic Database itself).");

                entity.Property(e => e.TermEntryId)
                    .HasColumnName("TermEntryID")
                    .HasComment("The ID of the entry (from LinguisticDatabaseTermEntries) of the term which is being translated");

                entity.Property(e => e.TermTextDraftTranslation).HasComment("The initial translation provided (NULL if the translator has not yet provided one).");

                entity.Property(e => e.TermTextDraftTranslationLastModifiedByEmployeeId)
                    .HasColumnName("TermTextDraftTranslationLastModifiedByEmployeeID")
                    .HasComment("If set, the ID of an internal employee if they were the last person to modify the translation. Normally we would expect the last person to be the supplier to whom we assigned the translation (in which case we would update TermTextTranslationLastModifiedByExtranetUserName), but we want to allow for other potential scenarios, e.g. if an internal employee needs to make a change while logged in as a test supplier or whatever.");

                entity.Property(e => e.TermTextDraftTranslationLastModifiedByExtranetUserName)
                    .HasMaxLength(100)
                    .HasComment("If set, the extranet user name of the last person to modify the translation. Normally this would be the supplier to whom we assigned the translation, but we want to allow for other potential scenarios, e.g. if we have to change translator half-way through, or if an internal employee needs to make a change while logged in as a test supplier or whatever.");

                entity.Property(e => e.TermTextDraftTranslationLastModifiedDateTime)
                    .HasColumnType("datetime")
                    .HasComment("The date/time when this translation was last modified/saved. If NULL, no one has actually changed it since it was sent to the supplier/translator. If a translator/supplier/internal employee made several changes during the translation process, this records the most recent change. If the status means that it is now being proofread post-translation, then this will log the final change during the translation, i.e. the state it was in when the translator signed it off.");

                entity.Property(e => e.TermTextPostProofreadTranslation).HasComment("A second version of the translation after the initial first translation but during/after any additional checking/proofreading. It is this version, once the status is final, which will be sent back to the main LinguisticDatabaseTermEntries table to then be Approved in the main database. At present it is anticipated that an internal employee would normally be checking the text before delivery to the client; if not, we may need to build some more logic into assigning this text to a new job item, etc.");

                entity.Property(e => e.TermTextPostProofreadTranslationLastModifiedByEmployeeId)
                    .HasColumnName("TermTextPostProofreadTranslationLastModifiedByEmployeeID")
                    .HasComment("If set, the ID of an internal employee if they were the last person to modify the translation. Normally we would expect the last person to be the project manager (or someone else internal checking on their behalf) who is checking through the translations before we deliver to the client.");

                entity.Property(e => e.TermTextPostProofreadTranslationLastModifiedByExtranetUserName)
                    .HasMaxLength(100)
                    .HasComment("If set, the extranet user name of the last person to modify the translation. In fact, normally this stage would be intended for our internal employees (project managers) to do the checking, in which case this field would typically be empty, but we want to allow for other potential scenarios, e.g. if we have to change translator half-way through or whatever.");

                entity.Property(e => e.TermTextPostProofreadTranslationLastModifiedDateTime)
                    .HasColumnType("datetime")
                    .HasComment("The date/time when this translation was last modified/saved. If NULL, no one has actually changed it since it was approved as a draft translation. If a translator/supplier/internal employee made several changes during the post-translation proofread process, this records the most recent change. If the status means that it has now been submitted as final, then this will log the final change during the post-translation proofread, i.e. the state it was in when finally signed off.");

                entity.Property(e => e.TranslationStatus).HasComment("At present 0 = being translated; 1 = has been translated by supplier, awaiting internal post-translation checking and signoff by a translate plus employee; 2 = has been checked post-translation, awaiting delivery back to the CMS; 3 = has been delivered back into the CMS content as \"Approved\" and therefore fully complete");
            });

            modelBuilder.Entity<ViewDataRequiredForComparingChangesBetweenRelease>(entity =>
            {
                entity.HasNoKey();

                entity.ToView("viewDataRequiredForComparingChangesBetweenReleases");

                entity.Property(e => e.ClientTermKeyId)
                    .HasMaxLength(50)
                    .HasColumnName("ClientTermKeyID");

                entity.Property(e => e.CmspublicationClientIdstring)
                    .IsRequired()
                    .HasMaxLength(200)
                    .HasColumnName("CMSPublicationClientIDString");

                entity.Property(e => e.CmspublicationId).HasColumnName("CMSPublicationID");

                entity.Property(e => e.CmspublicationName)
                    .IsRequired()
                    .HasMaxLength(200)
                    .HasColumnName("CMSPublicationName");

                entity.Property(e => e.CmspublicationReleaseId).HasColumnName("CMSPublicationReleaseID");

                entity.Property(e => e.EntryId).HasColumnName("EntryID");

                entity.Property(e => e.EntryTermText).IsRequired();

                entity.Property(e => e.LanguageIanacode)
                    .IsRequired()
                    .HasMaxLength(20)
                    .HasColumnName("LanguageIANACode");

                entity.Property(e => e.LinguisticDatabaseId).HasColumnName("LinguisticDatabaseID");

                entity.Property(e => e.TptermKeyId).HasColumnName("TPTermKeyID");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
