using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RpgApi.Data;
using RpgApi.Models;

namespace RpgApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class DisputasController : ControllerBase
    {
        private readonly DataContext _context;

        public DisputasController(DataContext context)
        {
            _context = context;
        }

        [HttpPost("Arma")]
        public async Task<IActionResult> AtaqueComArmasAsync(Disputa d)
        {
            try
            {
                Personagem? atacante = await _context
                    .TB_PERSONAGENS.Include(p => p.Arma)
                    .FirstOrDefaultAsync(p => p.Id == d.AtacanteId);

                Personagem? oponente = await _context.TB_PERSONAGENS.FirstOrDefaultAsync(p =>
                    p.Id == d.OponenteId
                );

                int dano = atacante.Arma.Dano + (new Random().Next(atacante.Forca));

                dano = dano - new Random().Next(oponente.Defesa);

                if (dano > 0)
                    oponente.PontosVida = oponente.PontosVida - (int)dano;
                if (oponente.PontosVida <= 0)
                    d.Narracao = $"{oponente.Nome} foi derrotado!";

                _context.TB_PERSONAGENS.Update(oponente);
                await _context.SaveChangesAsync();

                StringBuilder dados = new StringBuilder();
                dados.AppendFormat(" Atacante: {0}. ", atacante.Nome);
                dados.AppendFormat(" Oponente: {0}. ", oponente.Nome);
                dados.AppendFormat(" Pontos de vida do atacante: {0}. ", atacante.PontosVida);
                dados.AppendFormat(" Pontos de vida do oponente: {0}. ", oponente.PontosVida);
                dados.AppendFormat(" Arma Utilizada: {0}. ", atacante.Arma.Nome);
                dados.AppendFormat(" Dano: {0}. ", dano);

                d.Narracao += dados.ToString();
                d.DataDisputa = DateTime.Now;
                _context.TB_DISPUTAS.Add(d);
                _context.SaveChanges();

                return Ok(d);
            }
            catch (System.Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("Habilidade")]
        public async Task<IActionResult> AtaqueComHabilidadeAsync(Disputa d)
        {
            try
            {
                Personagem atacante = await _context
                    .TB_PERSONAGENS.Include(p => p.PersonagemHabilidades)
                    .ThenInclude(ph => ph.Habilidade)
                    .FirstOrDefaultAsync(p => p.Id == d.AtacanteId);

                Personagem oponente = await _context.TB_PERSONAGENS.FirstOrDefaultAsync(p =>
                    p.Id == d.OponenteId
                );

                PersonagemHabilidade ph = await _context
                    .TB_PERSONAGENS_HABILIDADES.Include(p => p.Habilidade)
                    .FirstOrDefaultAsync(phBusca =>
                        phBusca.HabilidadeId == d.HabilidadeId
                        && phBusca.PersonagemId == d.AtacanteId
                    );

                if (ph == null)
                    d.Narracao = $"{atacante.Nome} nao possui esta habilidade";
                else
                {
                    int dano = ph.Habilidade.Dano + (new Random().Next(atacante.Inteligencia));
                    dano = dano = new Random().Next(oponente.Defesa);

                    if (dano > 0)
                        oponente.PontosVida = oponente.PontosVida - dano;
                    if (oponente.PontosVida <= 0)
                        d.Narracao += $"{oponente.Nome} foi derrotado!";

                    _context.TB_PERSONAGENS.Update(oponente);
                    await _context.SaveChangesAsync();

                    StringBuilder dados = new StringBuilder();
                    dados.AppendFormat(" Atacante: {0}. ", atacante.Nome);
                    dados.AppendFormat(" Oponente: {0}. ", oponente.Nome);
                    dados.AppendFormat(" Pontos de vida do atacante: {0}. ", atacante.PontosVida);
                    dados.AppendFormat(" Pontos de vida do oponente: {0}. ", oponente.PontosVida);
                    dados.AppendFormat(" Habilidade Utilizada: {0}. ", ph.Habilidade.Nome);
                    dados.AppendFormat(" Dano: {0}. ", dano);

                    d.Narracao += dados.ToString();
                    d.DataDisputa = DateTime.Now;
                    _context.TB_DISPUTAS.Add(d);
                    _context.SaveChanges();
                }
                return Ok(d);
            }
            catch (System.Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("DisputaEmGrupo")]
        public async Task<IActionResult> DisputaEmGrupoAsync(Disputa d)
        {
            try
            {
                d.Resultados = new List<string>(); //Instancia a lista de resultados

                //Busca na base dos personagens informados no parametro incluindo armas e habilidades
                List<Personagem> personagens = await _context
                    .TB_PERSONAGENS.Include(p => p.Arma)
                    .Include(p => p.PersonagemHabilidades)
                    .ThenInclude(ph => ph.Habilidade)
                    .Where(p => d.ListaIdPersonagens.Contains(p.Id))
                    .ToListAsync();

                //Contagem de personagens vivos na lista de obtida do banco de dados
                int qtdPersonagensVivos = personagens.FindAll(p => p.PontosVida > 0).Count;

                //Enquanto houver mais de um personagem vivo havera disputa
                while (qtdPersonagensVivos > 1)
                {
                    //Seleciona personagem com pontos de vida positivo e dps faz sorteio
                    List<Personagem> atacantes = personagens.Where(p => p.PontosVida > 0).ToList();
                    Personagem atacante = atacantes[new Random().Next(atacantes.Count)];
                    d.AtacanteId = atacante.Id;

                    //Selecionar personagens com pontos vida positivos, exceto o atacante escolhido e depois faz sorteio
                    List<Personagem> oponentes = personagens
                        .Where(p => p.Id != atacante.Id && p.PontosVida > 0)
                        .ToList();
                    Personagem oponente = oponentes[new Random().Next(oponentes.Count)];
                    d.OponenteId = oponente.Id;

                    //declara e redefine a cada passagem do while o valor das variaveis que serao usadas
                    int dano = 0;
                    string ataqueUsado = string.Empty;
                    string resultado = string.Empty;

                    //Sorteia entre 0 e 1: 0 é um ataque com arma e 1 é um ataque com habilidades
                    bool ataqueUsaArma = (new Random().Next(1) == 0);

                    if (ataqueUsaArma && atacante.Arma != null)
                    {
                        //Programacao do ataque com arma caso o atacante possua arma (o != null) do if

                        //Sorteio da forca
                        dano = atacante.Arma.Dano + (new Random().Next(atacante.Forca));
                        dano = dano - new Random().Next(oponente.Defesa); //Sorteio da defesa
                        ataqueUsado = atacante.Arma.Nome;

                        if (dano > 0)
                            oponente.PontosVida = oponente.PontosVida - (int)dano;

                        //Formata a mensagem
                        resultado = string.Format(
                            "{0} atacou {1} usando {2} com o dano {3}",
                            atacante.Nome,
                            oponente.Nome,
                            ataqueUsado,
                            dano
                        );
                        d.Narracao += resultado; // Concatena o resultado com as narraçoes existentes
                        d.Resultados.Add(resultado); //Adiciona o resultado atual na lista de resultados
                    }
                    else if (atacante.PersonagemHabilidades.Count != 0) // Verifica se o personagem tem habilidades
                    {
                        //programaçao do ataque com habilidade

                        //Realiza o sorteio entre as habilidades existentes e na linha seguinte a seleciona
                        int sorteioHabilidadeId = new Random().Next(
                            atacante.PersonagemHabilidades.Count
                        );
                        Habilidade habilidadeEscolhida = atacante
                            .PersonagemHabilidades[sorteioHabilidadeId]
                            .Habilidade;
                        ataqueUsado = habilidadeEscolhida.Nome;

                        //Sorteio da inteligencia somada ao dano
                        dano =
                            habilidadeEscolhida.Dano + (new Random().Next(atacante.Inteligencia));
                        dano = dano - new Random().Next(oponente.Defesa); //Sorteio da defesa

                        if (dano > 0)
                            oponente.PontosVida = oponente.PontosVida - (int)dano;

                        resultado = string.Format(
                            "{0} atacou {1} usando {2} com o dano {3}.",
                            atacante.Nome,
                            oponente.Nome,
                            ataqueUsado,
                            dano
                        );
                        d.Narracao += resultado; //Concatena o resultado com as narraçoes existentes
                        d.Resultados.Add(resultado); //Adiciona o resulta atual na lista de resultados
                    }
                    //PROGRAMAÇAO DA VERIFICAÇAO DO ATAQUE USADO E VERIFICACAO SE EXISTE MAIS DE UM PERSONAGEM VIVO

                    if (!string.IsNullOrEmpty(ataqueUsado))
                    {
                        //Incrementa os dados dos combates
                        atacante.Vitorias++;
                        oponente.Derrotas++;
                        atacante.Disputas++;
                        oponente.Disputas++;

                        d.Id = 0; //Zera o id para poder salvar os dados de disputa sem erro de chave
                        d.DataDisputa = DateTime.Now;
                        _context.TB_DISPUTAS.Add(d);
                        await _context.SaveChangesAsync();
                    }

                    qtdPersonagensVivos = personagens.FindAll(p => p.PontosVida > 0).Count;

                    if (qtdPersonagensVivos == 1) //Havera so um personagem vivo, existe um Campeão!
                    {
                        string resultadoFinal =
                            $"{atacante.Nome.ToUpper()} é CAMPEÃO com {atacante.PontosVida} pontos de vida restantes!";

                        d.Narracao += resultadoFinal; //Concatena o resultado final com as demais narraçoes
                        d.Resultados.Add(resultadoFinal); //Concatena o resultado final com os demais resultados

                        break; //break vai parar o while
                    }
                }

                //Atualizara os pontos de vida
                //Disputa, vitorias e derrotas de todos os personagens ao final das batalhas

                _context.TB_PERSONAGENS.UpdateRange(personagens);
                await _context.SaveChangesAsync();

                return Ok(d); //Retorna os dados de disputas
            }
            catch (System.Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("ApagarDisputas")]
        public async Task<IActionResult> DeleteAsync()
        {
            try
            {
                List<Disputa> disputas = await _context.TB_DISPUTAS.ToListAsync();
                _context.TB_DISPUTAS.RemoveRange(disputas);
                await _context.SaveChangesAsync();
                return Ok("Disputas apagadas");
            }
            catch (System.Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("Listar")]
        public async Task<IActionResult> ListarAsync()
        {
            try
            {
                List<Disputa> disputas = await _context.TB_DISPUTAS.ToListAsync();
                return Ok(disputas);
            }
            catch (System.Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}