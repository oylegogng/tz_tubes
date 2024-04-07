using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using tz_tubes.Models;

namespace tz_tubes.Controllers
{
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _context;

        public HomeController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult AllPipes(bool? quality, int? steelGrade, double? size, double? weight, int? packetId)
        {
            var query = _context.Pipes.AsQueryable();

            // Применяем фильтр по качеству, если он указан
            if (quality.HasValue)
            {
                query = query.Where(p => (quality.Value == p.Quality));
            }

            // Применяем фильтр по марке стали, если она указана
            if (steelGrade.HasValue)
            {
                query = query.Where(p => p.SteelGrade == steelGrade.Value);
            }

            // Применяем фильтр по размеру, если он указан
            if (size.HasValue)
            {
                query = query.Where(p => p.Size == size.Value);
            }

            if (weight.HasValue)
            {
                query = query.Where(p => p.Weight == weight.Value);
            }

            // Применяем фильтр по номеру пакета, если он указан
            if (packetId.HasValue)
            {
                query = query.Where(p => p.PacketId == packetId.Value);
            }

            var pipes = query.ToList();


            var totalPipes = pipes.Count;
            var goodPipes = pipes.Count(p => p.Quality == true);
            var defectPipes = pipes.Count(p => p.Quality == false);
            var totalWeight = pipes.Sum(p => p.Weight);

            ViewBag.TotalPipes = totalPipes;
            ViewBag.GoodPipes = goodPipes;
            ViewBag.DefectPipes = defectPipes;
            ViewBag.TotalWeight = totalWeight;

            return View(pipes);
        }

        public IActionResult DeletePacket(int id)
        {
            var packet = _context.Packets.Find(id);
            if (packet == null)
            {
                return NotFound();
            }

            return View(packet);
        }

        [HttpPost]
        public IActionResult DeletePacketConfirmed(int id)
        {
            var packet = _context.Packets.Find(id);
            if (packet == null)
            {
                return NotFound();
            }

            _context.Packets.Remove(packet);
            _context.SaveChanges();
            return RedirectToAction("AllPackets");
        }
    


        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        public IActionResult AddPipe()
        {
            return View();
        }
        public IActionResult AddPacket()
        {
            
            return View();
        }
        [HttpPost]
        public IActionResult AddPacket(Packet packet)
        {
            if (ModelState.IsValid)
            {
                packet.CreationDate = DateTime.Now; // Устанавливаем текущую дату как дату создания пакета
                _context.Packets.Add(packet);
                _context.SaveChanges();
                return RedirectToAction("AllPackets"); // Перенаправляем пользователя на страницу всех пакетов после успешного добавления
            }

            return View(packet); // Если модель невалидна, возвращаем форму с сообщениями об ошибках
        }

        [HttpPost]
        public IActionResult AddPipe(Pipe pipe)
        {
            if (ModelState.IsValid)
            {
                // Проверяем, существует ли указанный пакет в базе данных
                if(pipe.PacketId != 0)
                {
                    var packetExists = _context.Packets.Any(p => p.PacketId == pipe.PacketId);
                    if (!packetExists)
                    {
                        ModelState.AddModelError(string.Empty, "Указанный пакет не существует");
                        return View(pipe);
                    }
                }
                
                if(pipe.Size <= 0 | pipe.SteelGrade <=0 | pipe.Weight <= 0)
                {
                    ModelState.AddModelError(string.Empty, "Проверьте корректность данных");
                    return View(pipe);
                }
                // Если все в порядке, добавляем трубу и перенаправляем пользователя на страницу "AllPipes"
                _context.Pipes.Add(pipe);
                _context.SaveChanges();
                return RedirectToAction("AllPipes");
            }

            // Если данные неверны, возвращаем ту же форму с сообщениями об ошибках
            return View(pipe);
        }



        public IActionResult DeletePipe(int id)
        {
            var pipe = _context.Pipes.Find(id);
            if (pipe == null)
            {
                return NotFound();
            }

            return View(pipe);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            var pipe = _context.Pipes.Find(id);
            if (pipe == null)
            {
                return NotFound();
            }

            _context.Pipes.Remove(pipe);
            _context.SaveChanges();

            return RedirectToAction("AllPipes");
        }



        public IActionResult EditPipe(int id)
        {
            var pipe = _context.Pipes.Find(id);
            if (pipe == null)
            {
                return NotFound();
            }

            return View(pipe);
        }

        [HttpPost]
        public IActionResult EditPipe(Pipe pipe)
        {
            // Получаем оригинальные данные из базы данных для трубы
            var originalPipe = _context.Pipes.AsNoTracking().FirstOrDefault(p => p.PipeId == pipe.PipeId);

            // Если номер пакета изменился, сохраняем только его значение, остальные поля оставляем без изменений
            if (originalPipe != null && originalPipe.PacketId != pipe.PacketId)
            {
                pipe.Quality = originalPipe.Quality;
                pipe.SteelGrade = originalPipe.SteelGrade;
                pipe.Size = originalPipe.Size;
                pipe.Weight = originalPipe.Weight;
            }

            if (ModelState.IsValid)
            {
                // Проверяем, существует ли указанный пакет в базе данных
                if (pipe.PacketId != 0)
                {
                    var packetExists = _context.Packets.Any(p => p.PacketId == pipe.PacketId);
                    if (!packetExists)
                    {
                        ModelState.AddModelError(string.Empty, "Указанный пакет не существует");
                        return View(pipe);
                    }
                }

                if (pipe.Size <= 0 | pipe.SteelGrade <= 0 | pipe.Weight <= 0)
                {
                    ModelState.AddModelError(string.Empty, "Проверьте корректность данных");
                    return View(pipe);
                }
                _context.Update(pipe);
                _context.SaveChanges();
                return RedirectToAction("AllPipes");

                return RedirectToAction("AllPipes"); // Перенаправляем пользователя на страницу "AllPipes"
            }

            // Если данные неверны, возвращаем ту же форму с сообщениями об ошибках
            return View(pipe);
        }

       

        public IActionResult AllPackets()
        {
            var packets = _context.Packets.ToList();
            return View(packets);
        }



        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}