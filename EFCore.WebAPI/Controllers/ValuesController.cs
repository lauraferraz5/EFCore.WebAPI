using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EFCore.Dominio;
using EFCore.Repo;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EFCore.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]

    public class ValuesController : ControllerBase
    {
        public readonly HeroiContext _context;

        // public HeroiContext _context { get; set; }

        public ValuesController(HeroiContext context)
        {
            _context = context;
        }

        // GET api/values
        [HttpGet]
        public ActionResult Get()
        {
            // LINQ Method
            var listHeroi = _context.Herois.ToList();

            // LINQ Query
            //var listHeroi = (from heroi in _context.Herois select heroi).ToList();  

            return Ok(listHeroi);
        }

        // Usando o método .where()
        // GET api/values
        [HttpGet("filtro2/{nome}")]
        public ActionResult GetFiltro(string nome)
        {
            // LINQ Method
            var listHeroi = _context.Herois.Where(h => h.Nome.Contains(nome)).ToList(); // O ToList fecha a conexão

            // LINQ Query
            //var listHeroi = (from heroi in _context.Herois where heroi.Nome.Contains(nome) select heroi).ToList();  

            // Se fosse necessário fazer um foreach, por exemplo, faz da variável que guarda o resultado da consulta e não na consulta em si, para não deixar a conexão aberta
            //foreach (var item in listHeroi) // coloca o listHeroi em vez da coleção toda
            //{
            //    realizaCalculo();
            //    criaArquivos();
            //    salvaRelatorio();
            //}

            return Ok(listHeroi);
        }

        // GET api/values
        // Usando EF Functions
        [HttpGet("filtro/{nome}")]
        public ActionResult GetFiltroEF(string nome)
        {
            // LINQ Method
            var listHeroi = _context.Herois.Where(h => EF.Functions.Like(h.Nome, $"%{nome}%")).OrderBy(h => h.Id).LastOrDefault();

            // LINQ Query
            //var listHeroi = (from heroi in _context.Herois where heroi.Nome.Contains(nome) select heroi).ToList();  

            return Ok(listHeroi);
        }

        // GET api/values/5
        [HttpGet("{nameHero}")]
        public ActionResult Get(string nameHero)
        {
            var heroi = new Heroi { Nome = nameHero };

            _context.Herois.Add(heroi);
            _context.SaveChanges();

            return Ok();
        }

        // GET api/values/5
        [HttpGet("Atualizar/{nameHero}")]
        public ActionResult GetAtt(string nameHero)
        {
            var heroi = _context.Herois.Where(h => h.Id == 1).FirstOrDefault();

            heroi.Nome = "Homem Aranha";

            _context.SaveChanges();

            return Ok();
        }

        // GET api/values/5
        [HttpGet("AddRange")]
        public ActionResult GetAddRange()
        {
            _context.AddRange(
                new Heroi { Nome = "Capitão América" },
                new Heroi { Nome = "Doutor Estranho" },
                new Heroi { Nome = "Pantera Negra" },
                new Heroi { Nome = "Viúva Negra" },
                new Heroi { Nome = "Hulk" },
                new Heroi { Nome = "Gavião Arqueiro" },
                new Heroi { Nome = "Capitã Marvel" }
                );

            _context.SaveChanges();

            return Ok();
        }

        // DELETE api/values/5
        [HttpGet("Delete/{id}")]
        public void Delete(int id)
        {
            var heroi = _context.Herois.Where(x => x.Id == id).Single();

            _context.Herois.Remove(heroi);
            _context.SaveChanges();
        }
    }
}
