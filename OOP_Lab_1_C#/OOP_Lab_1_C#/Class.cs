using System;

namespace lab_1
{

    public class Client
    {
        public string name;
        public Gender gender;
        private string address;
        private float bonus;
        public Status status;
        public DateOnly birthDate;


        public void setadress(string adress)
        {
            this.address = adress;
        }


        public string getaddress() { return this.address; }


        public void setbonus(float bonus)
        {
            this.bonus = bonus;
        }


        public float getbonus() { return this.bonus; }


        public void addBonus(float price)
        {
            this.bonus += price;
        }


        public float[] getDiscount(float price) //max discount 10%
        {
            float bonus;
            float[] result = new float[3];
            bonus = this.bonus - price;
            if (bonus >= 0)
            {
                result[0] = price - (price / 10); //Сума зі знижкою
                result[1] = price; // Витрачені бонуси
                result[2] = bonus; // Залишок бонусів
                this.bonus = bonus;
            }
            else 
            {
                result[0] = price - (this.bonus / 10); //Сума зі знижкою
                result[1] = this.bonus; // Витрачені бонуси
                result[2] = 0; // Залишок бонусів
                this.bonus = 0;
            }
            return result;
        }


        public string[] buyByBonus (float price)
        {
            string[] result = new string[3];
            if ((this.bonus / 10) >= price)
            {
                this.bonus -= price * 10;
                result[0] = "true";
            }
            else 
            {
                result[0] = "false";
            }

            result[1] = $"{price * 10}";
            result[2] = $"{this.bonus}";
            
            return result;
        }
    };
}