namespace HelpDesk.Models.ViewModels
{
    public class CatPlazasAreasViewModel
    {
        public List<CatPlazaViewModel>? Plazas { get; set; }
        public List<CatAreaViewModel>? Areas { get; set; }
    }

    public class CatPlazaAreaViewModel
    {
        public CatPlazaViewModel? Plaza { get; set; }
        public CatAreaViewModel? Area { get; set; }

        public List<CatPlazaViewModel>? CatalogoPlazas { get; set; }
    }
}
