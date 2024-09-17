using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Bus;
public record ProductAddedEvent(int Id, string Name, decimal Price);
