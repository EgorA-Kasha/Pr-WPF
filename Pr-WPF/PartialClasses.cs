using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Pr_WPF;

namespace Pr_WPF
{
    public partial class basepart
    {
        public string Characteristics
        {
            get
            {
                int currentTypeId = this.parttypeid;

                if (currentTypeId == 1)
                {
                    var item = App.db.cpu.FirstOrDefault(x => x.id == this.id);
                    if (item != null)
                    {
                        var s = App.db.socket.FirstOrDefault(x => x.id == item.socketid);
                        return $"Сокет: {s?.name}, {item.numberofcores} ядер, {item.basecorefrequency}-{item.maxcorefrequency} ГГц, L3: {item.cachel3}МБ, TDP: {item.thermalpower}Вт";
                    }
                }
                if (currentTypeId == 2)
                {
                    var item = App.db.gpu.FirstOrDefault(x => x.id == this.id);
                    if (item != null)
                    {
                        var i = App.db.gpuinterface.FirstOrDefault(x => x.id == item.gpuinterfaceid);
                        return $"{i?.name}, {item.videomemory}ГБ, {item.memorybus} бит, {item.chipfrequency}МГц, БП от {item.recommendpower}Вт";
                    }
                }
                if (currentTypeId == 3)
                {
                    var item = App.db.ram.FirstOrDefault(x => x.id == this.id);
                    if (item != null)
                    {
                        var m = App.db.memorytype.FirstOrDefault(x => x.id == item.memorytypeid);
                        return $"{m?.name}, {item.count}x{item.capacity}ГБ, {item.ghz}МГц, {item.timings}";
                    }
                }
                if (currentTypeId == 4)
                {
                    var item = App.db.motherboard.FirstOrDefault(x => x.id == this.id);
                    if (item != null)
                    {
                        var s = App.db.socket.FirstOrDefault(x => x.id == item.socketid);
                        var f = App.db.formfactor.FirstOrDefault(x => x.id == item.formfactorid);
                        var m = App.db.memorytype.FirstOrDefault(x => x.id == item.memorytypeid);
                        return $"{f?.name}, Сокет: {s?.name}, ОЗУ: {m?.name} ({item.memoryslots} шт), PCI: {item.pcislots}, SATA: {item.sataports}";
                    }
                }
                if (currentTypeId == 5)
                {
                    var item = App.db.@case.FirstOrDefault(x => x.id == this.id);
                    if (item != null)
                    {
                        var s = App.db.casesize.FirstOrDefault(x => x.id == item.sizeid);
                        return $"{s?.name}, Слотов: {item.expansionslots}, Вентиляторов: {item.fans}";
                    }
                }
                if (currentTypeId == 6)
                {
                    var item = App.db.powersupply.FirstOrDefault(x => x.id == this.id);
                    if (item != null)
                    {
                        var c = App.db.certificate.FirstOrDefault(x => x.id == item.certificationid);
                        return $"{item.power}Вт, Сертификат: {c?.name}";
                    }
                }
                if (currentTypeId == 7)
                {
                    var item = App.db.processorcooler.FirstOrDefault(x => x.id == this.id);
                    if (item != null)
                    {
                        var f = App.db.fandimension.FirstOrDefault(x => x.id == item.fandimensionid);
                        return $"Вентилятор: {f?.name}, {item.heatpipes} трубок, {item.minspeed}-{item.maxspeed} RPM, {item.noiselevel} дБ";
                    }
                }
                if (currentTypeId == 8)
                {
                    var item = App.db.storagedevice.FirstOrDefault(x => x.id == this.id);
                    if (item != null)
                    {
                        var t = App.db.storagedevicetype.FirstOrDefault(x => x.id == item.storagedevicetypeid);
                        var i = App.db.storagedeviceinterface.FirstOrDefault(x => x.id == item.storagedeviceinterfaceid);
                        return $"{t?.name}, {item.capacity}ГБ, {i?.name}";
                    }
                }
                return "";
            }
        }
    }
}
