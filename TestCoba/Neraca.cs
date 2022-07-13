using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TestCoba
{
    [Serializable]
    public class Neraca
    {
        private int _idNeraca;
        private string _jenisNeraca;
        private decimal _nilaiPenjualan;
        private decimal _nilaiHPP;
        private decimal _nilaiLabaKotor;
        private decimal _nilaiSelisihQty;
        private decimal _nilaiTotalBiaya;
        private decimal _nilaiJumlahLabaDanBiaya;
        private decimal _nilaiTotalLaba;
        private decimal _nilaiTotalPajak;
        private decimal _nilaiTotalSetelahPajak;
        private decimal _nilaiTotalSetelahRounding;

        public decimal NilaiTotalSetelahRounding
        {
            get { return _nilaiTotalSetelahRounding; }
            set { _nilaiTotalSetelahRounding = value; }
        }

        public decimal NilaiTotalSetelahPajak
        {
            get { return _nilaiTotalSetelahPajak; }
            set { _nilaiTotalSetelahPajak = value; }
        }

        public decimal NilaiTotalPajak
        {
            get { return _nilaiTotalPajak; }
            set { _nilaiTotalPajak = value; }
        }

        public decimal NilaiTotalLaba
        {
            get { return _nilaiTotalLaba; }
            set { _nilaiTotalLaba = value; }
        }

        public decimal NilaiJumlahLabaDanBiaya
        {
            get { return _nilaiJumlahLabaDanBiaya; }
            set { _nilaiJumlahLabaDanBiaya = value; }
        }

        public decimal NilaiTotalBiaya
        {
            get { return _nilaiTotalBiaya; }
            set { _nilaiTotalBiaya = value; }
        }

        public decimal NilaiLabaKotor
        {
            get { return _nilaiLabaKotor; }
            set { _nilaiLabaKotor = value; }
        }

        public decimal NilaiHPP
        {
            get { return _nilaiHPP; }
            set { _nilaiHPP = value; }
        }

        public decimal NilaiPenjualan
        {
            get { return _nilaiPenjualan; }
            set { _nilaiPenjualan = value; }
        }

        public string JenisNeraca
        {
            get { return _jenisNeraca; }
            set { _jenisNeraca = value; }
        }

        public int IdNeraca
        {
            get { return _idNeraca; }
            set { _idNeraca = value; }
        }

        public decimal NilaiSelisihQty { get => _nilaiSelisihQty; set => _nilaiSelisihQty = value; }
    }
}