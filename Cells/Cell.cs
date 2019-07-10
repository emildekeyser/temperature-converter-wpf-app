using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cells
{
    public class Cell<T> : INotifyPropertyChanged
    {
        T contents;

        public Cell(T initialContents = default(T))
        {
            this.contents = initialContents;
        }

        public virtual T Value
        {
            get
            {
                return this.contents;
            }
            set
            {
                this.contents = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(this.Value)));
            }
        }

        public Cell<U> Derive<U>(Func<T, U> transformer, Func<U, T> untransformer)
        {
            return new Derived<T, U>(this, transformer, untransformer);
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }

    public class Derived<IN, OUT> : Cell<OUT>
    {
        private readonly Cell<IN> dependancy;
        private readonly Func<IN, OUT> transformer;
        private readonly Func<OUT, IN> untransformer;

        public Derived(Cell<IN> dependancy, Func<IN, OUT> transformer, Func<OUT, IN> untransformer) : base(transformer(dependancy.Value))
        {
            this.dependancy = dependancy;
            this.transformer = transformer;
            this.untransformer = untransformer;
            this.dependancy.PropertyChanged += (sender, args) => base.Value = transformer(dependancy.Value);
        }

        public override OUT Value
        {
            get => base.Value;
            set => this.dependancy.Value = untransformer(value);
        }
    }
}
