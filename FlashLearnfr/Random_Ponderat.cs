using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FlashLearnfr
{
    class Random_Ponderat
    {
        private Random random;
        private List<int> valori;
        private List<float> ponderi;
        public Random_Ponderat(List<int> valori, List<float> ponderi)
        {
            random = new Random();
            this.valori = valori;
            this.ponderi = ponderi;
        }
        public int getValue(){
          int total=0;
            foreach(int i in ponderi)
                total+=i;
            int r = random.Next(total);
            float curent=0;
            for(int i=0;i<ponderi.Count;i++){
                curent+=ponderi[i];
                if (r < curent)
                    return valori[i];
               
            }
            return -1;
        }
    }
}
