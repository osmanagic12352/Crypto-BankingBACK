using Crypto_BankingREG.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Crypto_BankingREG.Models.Service
{
    public class TransakcijaService
    {
        private MainContext _context;

        public TransakcijaService(MainContext context)
        {
            _context = context;
        }

        public void AddTransakciju(TransakcijaView transakcije)
        {
            var _transakcije = new TransakcijaModel()
            {
                NazivValute = transakcije.NazivValute,
                Kolicina = transakcije.Kolicina,
                VrstaTransakcije = transakcije.VrstaTransakcije,
                CryptoAdresa = transakcije.CryptoAdresa
            };
            _context.Transakcija.Add(_transakcije);
            _context.SaveChanges();
        }

        public List<TransakcijaModel> GetAllTransakcije()
        {
            var AllTransakcije = _context.Transakcija.ToList();
            return AllTransakcije;
        }

        public TransakcijaModel GetTransakcijuById(int TransakcijaID)
        {
            var transakcija = _context.Transakcija.FirstOrDefault(n => n.TransakcijaId == TransakcijaID);
            return transakcija;
        }

        public TransakcijaModel UpdateTransakcijuById(int TransakcijaID, TransakcijaView transakcija)
        {
            var _transakcije = _context.Transakcija.FirstOrDefault(n => n.TransakcijaId == TransakcijaID);
            if (_transakcije != null)
            {
                _transakcije.NazivValute = transakcija.NazivValute;
                _transakcije.Kolicina = transakcija.Kolicina;
                _transakcije.VrstaTransakcije = transakcija.VrstaTransakcije;
                _transakcije.CryptoAdresa = transakcija.CryptoAdresa;

                _context.SaveChanges();
            }
            return _transakcije;
        }

        public void DeleteTransakcijuById(int TransakcijaID)
        {
            var _transakcija = _context.Transakcija.FirstOrDefault(n => n.TransakcijaId == TransakcijaID);
            if (_transakcija != null)
            {
                _context.Transakcija.Remove(_transakcija);
                _context.SaveChanges();
            }
        }
    }

}
