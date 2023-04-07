using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public interface IHealthSystem
{
    public float MaxHeatlh { get; }
    public float CurrentHealth { get; }
    public event System.Action onHpChanged;
}