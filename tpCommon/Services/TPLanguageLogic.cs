using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Services.Interfaces;
using Data;
using Data.Repositories;
using Microsoft.EntityFrameworkCore;


namespace Services
{
    public class TPLanguageLogic : ITPLanguageLogic
    {
        private readonly IRepository<LocalLanguageInfo> localLangRepo;
        private readonly IRepository<Language> langRepo;

        public TPLanguageLogic(IRepository<LocalLanguageInfo> repository, IRepository<Language> repository1)
        {
            this.localLangRepo = repository;
            this.langRepo = repository1;
        }

        public LocalLanguageInfo GetLanguageInfo(string languageCodeBeingDescribed, string IANACode)
        {
            var result = localLangRepo.All().Where(lo => lo.LanguageIanacode == IANACode && lo.LanguageIanacodeBeingDescribed == languageCodeBeingDescribed)
                                                        .Join(langRepo.All().Where(l => l.Ianacode == languageCodeBeingDescribed),
                                                        lo => lo.LanguageIanacodeBeingDescribed,
                                                        l => l.Ianacode,
                                                        (lo, l) => new { localLang = lo }).Select(l => l.localLang).FirstOrDefault();

            return result;
        }
        public string MapLangIanaCodeWithRegion(string IanaCode)
        {

            Dictionary<string, string> AllLanguages = new Dictionary<string, string>();

            AllLanguages.Add("en", "en-GB");
            AllLanguages.Add("da", "da-DK");
            AllLanguages.Add("de-de", "de-DE");
            AllLanguages.Add("de", "de-DE");
            AllLanguages.Add("it", "it-IT");
            AllLanguages.Add("no", "nb-NO");
            AllLanguages.Add("nn", "nb-NO");
            AllLanguages.Add("ar", "ar-AE");
            AllLanguages.Add("es-005", "es-419");
            AllLanguages.Add("zh-hans", "zh-CN");
            AllLanguages.Add("zh-hant", "zh-TW");
            AllLanguages.Add("zh-yue", "zh-MO");
            AllLanguages.Add("cs", "cs-CZ");
            AllLanguages.Add("ja", "ja-JP");
            AllLanguages.Add("vi", "vi-VN");
            AllLanguages.Add("pu", "pa-IN");
            AllLanguages.Add("pa", "pa-Arab-PK");
            AllLanguages.Add("ku", "ku-Arab-IR");
            AllLanguages.Add("or", "or-IN");
            AllLanguages.Add("ur", "ur-PK");
            AllLanguages.Add("bg", "bg-BG");
            AllLanguages.Add("cy", "cy-GB");
            AllLanguages.Add("el", "el-GR");
            AllLanguages.Add("es", "es-ES");
            AllLanguages.Add("et", "et-EE");
            AllLanguages.Add("fi", "fi-FI");
            AllLanguages.Add("fil", "fil-PH");
            AllLanguages.Add("fr", "fr-FR");
            AllLanguages.Add("gu", "gu-IN");
            AllLanguages.Add("hi", "hi-IN");
            AllLanguages.Add("hr", "hr-HR");
            AllLanguages.Add("hu", "hu-HU");
            AllLanguages.Add("id", "id-ID");
            AllLanguages.Add("is", "is-IS");
            AllLanguages.Add("kk", "kk-KZ");
            AllLanguages.Add("kn", "kn-IN");
            AllLanguages.Add("ko", "ko-KR");
            AllLanguages.Add("lt", "lt-LT");
            AllLanguages.Add("lv", "lv-LV");
            AllLanguages.Add("mk", "mk-MK");
            AllLanguages.Add("ml", "ml-IN");
            AllLanguages.Add("ms", "ms-MY");
            AllLanguages.Add("mt", "mt-MT");
            AllLanguages.Add("nl", "nl-NL");
            AllLanguages.Add("pl", "pl-PL");
            AllLanguages.Add("pt", "pt-PT");
            AllLanguages.Add("ro", "ro-RO");
            AllLanguages.Add("ru", "ru-RU");
            AllLanguages.Add("sk", "sk-SK");
            AllLanguages.Add("sl", "sl-SI");
            AllLanguages.Add("sr", "sr-Latn-RS");
            AllLanguages.Add("sv", "sv-SE");
            AllLanguages.Add("ta", "ta-IN");
            AllLanguages.Add("te", "te-IN");
            AllLanguages.Add("th", "th-TH");
            AllLanguages.Add("tl", "tl-x-Sdl");
            AllLanguages.Add("tr", "tr-TR");
            AllLanguages.Add("uk", "uk-UA");
            AllLanguages.Add("nb", "nb-NO");
            AllLanguages.Add("he", "he-IL");
            AllLanguages.Add("my", "my-MM");
            AllLanguages.Add("bn", "bn-IN");
            AllLanguages.Add("bn-SYL", "bn-IN");
            AllLanguages.Add("fa", "fa-IR");
            AllLanguages.Add("am", "am-ET");

            if (AllLanguages.ContainsKey(IanaCode.ToLower()))
            {
                string result = string.Empty;
                AllLanguages.TryGetValue(IanaCode.ToLower(), out result);
                return result;
            }

            return IanaCode;
        }
    }
}
