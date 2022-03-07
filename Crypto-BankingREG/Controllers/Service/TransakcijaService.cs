using AutoMapper;
using Crypto_BankingREG.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Crypto_BankingREG.Models.Service
{
    public class TransakcijaService
    {
        private DBContext _context;
        private readonly IMapper _mapper;

        public TransakcijaService(DBContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public void AddTransakciju(TransakcijaView transakcije, string UserID)
        {
            var _user = _context.ApplicationUsers.FirstOrDefault(n => n.Id == UserID);
            if (_user != null)
            {
                var _transakcije = _mapper.Map<TransakcijaModel>(transakcije);
                _transakcije = new TransakcijaModel()
                {
                    NazivValute = transakcije.NazivValute,
                    Kolicina = transakcije.Kolicina,
                    VrstaTransakcije = transakcije.VrstaTransakcije,
                    CryptoAdresa = transakcije.CryptoAdresa,
                    UserId = UserID
                };
                if (_transakcije != null)
                {
                    _context.Transakcija.Add(_transakcije);
                    _context.SaveChanges();
                }
                else
                {
                    throw new Exception("Transakcija neuspješna!");
                }
            }
            else
            {
                throw new Exception("Unjeli ste nepostojeći Id korisnika ili se isti već koristi!");
            }
        }

        public List<TransakcijaModel> GetAllTransakcije()
        {
            var AllTransakcije = _context.Transakcija.ToList();
            return AllTransakcije;
        }

        public TransakcijaModel GetTransakcijuById(int TransakcijaID)
        {
            var transakcija = _context.Transakcija.FirstOrDefault(n => n.TransakcijaId == TransakcijaID);
            if (transakcija != null)
            {
                return transakcija;
            }
            else
            {
                throw new Exception("Pogrešan Id transakcije!");
            }
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
            else
            {
                throw new Exception("Pogrešan Id transakcije");
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
            else
            {
                throw new Exception("Uneseni Id je neispravan");
            }
        }
    }

}
