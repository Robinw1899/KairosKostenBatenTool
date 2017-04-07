namespace KairosWeb_Groep6.Models.Domain
{
    public interface IIntroductietekstRepository
    {
        Introductietekst GetIntroductietekst();
        void Add(Introductietekst tekst);
        void Remove(Introductietekst tekst);
        void Save();
    }
}
