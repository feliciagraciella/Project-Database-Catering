using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.MySqlClient;
using System.IO;
using Microsoft.VisualBasic;

namespace App_Perusahaan
{
    public partial class FormMenu : Form
    {
        public FormMenu()
        {
            InitializeComponent();
        }

        FormLogin fLogin;

        public void init(FormLogin f)
        {
            fLogin = f;
        }
        MySqlDataAdapter sqlAdapter;
        string sqlQuery;
        DataTable dtMenu = new DataTable();
        DataTable dtBulan = new DataTable();
        DataTable dtPembeliLama = new DataTable();
        DataTable dtMenuList = new DataTable();
        string newPath;
        int pencet;
        FileStream fs;
        BinaryReader br;
        byte[] ImageData;
        int subtotal = 0;
        int total = 0;
        DataTable dtpb = new DataTable();

        private void FormMenu_Load(object sender, EventArgs e)
        {
            try
            {
                gBoxTransaksi.Visible = false;
                groupBoxMenu.Visible = true;
                panelMenu.Visible = false;
                rbAllMenu.Checked = true;
                ShowAllMenu();
                IsiDt();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void dgvMenu_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                pencet = 1;
                tBoxID.Text = dgvMenu.Rows[e.RowIndex].Cells[0].Value.ToString();
                rbMakanan.Enabled = false;
                rbMinuman.Enabled = false;
                buttonUpdate.Text = "Update";
                EditMenu();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private void rbAllMenu_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                if (rbAllMenu.Checked == true)
                {
                    dtpMenu.Enabled = false;
                    ShowAllMenu();
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private void rbDaily_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                if (rbDaily.Checked == true)
                {
                    dtpMenu.Enabled = true;
                    ShowDailyMenu();
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private void dtpMenu_ValueChanged(object sender, EventArgs e)
        {
            try
            {
                ShowDailyMenu();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private void buttonAddMenu_Click(object sender, EventArgs e)
        {
            try
            {
                pencet = 2;
                rbMakanan.Checked = false;
                rbMinuman.Checked = false;
                tBoxID.Text = "";
                buttonUpdate.Text = "Add";
                InputMenu();

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private void rbMakanan_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                if (pencet == 2)
                {
                    //IsiID();
                    IsiIdmkn();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private void rbMinuman_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                if (pencet == 2)
                {
                    //IsiID();
                    IsiIdmnm();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private void labelChange_Click(object sender, EventArgs e)
        {
            try
            {
                OpenFileDialog openFD = new OpenFileDialog();
                openFD.Filter = "Image Files (*.jpg;*.jpeg;*.png;)|*.jpg;*.jpeg;*.png;";
                if (openFD.ShowDialog() == DialogResult.OK)
                {
                    pbMenu.Image = new Bitmap(openFD.FileName);
                }
                var path = openFD.FileName;
                newPath = path.Replace("\\", "\\\\");

                fs = new FileStream(newPath, FileMode.Open, FileAccess.Read);
                br = new BinaryReader(fs);
                ImageData = br.ReadBytes((int)fs.Length);
                br.Close();
                fs.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private void buttonDel_Click(object sender, EventArgs e)
        {
            try
            {
                DialogResult dialogResult = MessageBox.Show("are you sure?", "Delete", MessageBoxButtons.YesNo);
                if (dialogResult == DialogResult.Yes)
                {
                    MySqlCommand sqlCommand = new MySqlCommand();
                    sqlQuery = "update menu set menu_delete = 1 where id_menu = '" + tBoxID.Text.ToString() + "'";
                    sqlCommand = new MySqlCommand(sqlQuery, fLogin.conn);
                    sqlCommand.ExecuteNonQuery();
                }
                FormMenu_Load(sender, e);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private void buttonCancel_Click(object sender, EventArgs e)
        {
            try
            {
                DialogResult dialogResult = MessageBox.Show("Discard Change?", "Cancel", MessageBoxButtons.YesNo);
                if (dialogResult == DialogResult.Yes)
                {
                    if (pencet == 1)
                    {
                        EditMenu();
                    }
                    else if (pencet == 2)
                    {
                        InputMenu();
                    }
                }
                else if (dialogResult == DialogResult.No)
                {
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private void buttonUpdate_Click(object sender, EventArgs e)
        {
            try
            {
                if (buttonUpdate.Text == "Add")
                {
                    if (tBoxMenu.Text == "" || tBoxDesc.Text == "" || tBoxHarga.Text == "" || cboxBulan.SelectedIndex == -1 /*|| newPath == null*/ || cboxTahun.SelectedIndex == -1 || tBoxTanggal.Text == "")
                    {
                        MessageBox.Show("Ada data yang masih kosong");
                    }
                    else
                    {
                        MySqlCommand sqlCommand = new MySqlCommand();
                        sqlCommand.Connection = fLogin.conn;
                        sqlQuery = "insert into menu values('" + tBoxID.Text.ToString() + "', '" + tBoxHarga.Text.ToString() + "','" + tBoxMenu.Text.ToString() + "', '" + ImageData + "', '" + 
                            tBoxDesc.Text.ToString() + "', 0, 0)";

                        DataTable dt = new DataTable();
                        sqlCommand = new MySqlCommand(sqlQuery, fLogin.conn);
                        sqlCommand.ExecuteNonQuery();


                        sqlQuery = "select j.id_jadwal as `ID Jadwal` from jadwal j where j.bulan = '" + cboxBulan.Text.ToString() + "' and j.tahun = '" + cboxTahun.Text.ToString() + "'";
                        DataTable dtJadwal = new DataTable();
                        sqlCommand = new MySqlCommand(sqlQuery, fLogin.conn);
                        sqlAdapter = new MySqlDataAdapter(sqlCommand);
                        sqlAdapter.Fill(dtJadwal);
                        string idjadwal = dtJadwal.Rows[0]["ID Jadwal"].ToString();

                        sqlQuery = "insert into jadwal_catering values('" + tBoxID.Text.ToString() + "', '" + idjadwal + "', '" + tBoxTanggal.Text.ToString() + "', 0)";
                        sqlCommand = new MySqlCommand(sqlQuery, fLogin.conn);
                        sqlCommand.ExecuteNonQuery();

                        MessageBox.Show("Data sudah diinput");
                        ShowAllMenu();
                        FormMenu_Load(sender, e);
                    }
                }
                else if (buttonUpdate.Text == "Update")
                {
                    if (tBoxMenu.Text == "" || tBoxDesc.Text == "" || tBoxHarga.Text == "" || cboxBulan.SelectedIndex == -1 || cboxTahun.SelectedIndex == -1 || tBoxTanggal.Text == "")
                    {
                        MessageBox.Show("Ada data yang masih kosong");
                    }
                    else
                    {
                        if (tBoxMenu.Text == dtMenu.Rows[0]["Nama Menu"].ToString() && tBoxDesc.Text == dtMenu.Rows[0]["Deskripsi"].ToString() && tBoxHarga.Text == dtMenu.Rows[0]["Harga"].ToString() && 
                            cboxBulan.Text == dtMenu.Rows[0]["Bulan"].ToString() && cboxTahun.Text == dtMenu.Rows[0]["Tahun"].ToString() && tBoxTanggal.Text == dtMenu.Rows[0]["Tanggal"].ToString() && 
                            pbMenu.Image == App_Perusahaan.Properties.Resources.dish_0)
                        {
                            MessageBox.Show("Belum ada data yang diubah");
                        }
                        else
                        {
                            MySqlCommand sqlCommand = new MySqlCommand();
                            sqlCommand.Connection = fLogin.conn;
                            sqlQuery = "update menu set nama_menu = '" + tBoxMenu.Text.ToString() + "', deskripsi_menu = '" + tBoxDesc.Text.ToString() + "', harga_menu = '" + 
                                tBoxHarga.Text.ToString() + "' where id_menu = '" + tBoxID.Text.ToString() + "'";
                            sqlCommand = new MySqlCommand(sqlQuery, fLogin.conn);
                            sqlCommand.ExecuteNonQuery();
                            

                            if (newPath != null)
                            {
                                sqlQuery = "update menu set foto_menu = '" + ImageData + "' where id_menu = '" + tBoxID.Text.ToString() + "'";
                                sqlCommand = new MySqlCommand(sqlQuery, fLogin.conn);
                                sqlCommand.ExecuteNonQuery();
                                
                            }

                            sqlQuery = "update jadwal_catering jc, jadwal j set jc.tanggal = '" + tBoxTanggal.Text + "', jc.id_jadwal = j.id_jadwal where j.bulan = '" + 
                                cboxBulan.Text.ToString() + "' and j.tahun = '" + cboxTahun.Text.ToString() + "' and jc.id_menu = '" + tBoxID.Text.ToString() + "'";
                            sqlCommand = new MySqlCommand(sqlQuery, fLogin.conn);
                            sqlCommand.ExecuteNonQuery();
                            

                            MessageBox.Show("Data sudah diupdate");
                            ShowAllMenu();
                            FormMenu_Load(sender, e);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private void buttonTransaksi_Click(object sender, EventArgs e)
        {
            try
            {
                gBoxLaporan.Visible = false;
                gBoxTransaksi.Visible = true;
                groupBoxMenu.Visible = false;
                gBoxPesanan.Visible = true;

                rbLama.Checked = false;
                rbBaru.Checked = true;
                cbPembeli.Visible = false;
                tbNamaPembeli.Visible = true;


                GenIDTB();
                IsiMenu();
                dgvDftPesan.Rows.Clear();
                
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private void buttonMenu_Click(object sender, EventArgs e)
        {
            try
            {
                gBoxLaporan.Visible = false;
                gBoxTransaksi.Visible = false;
                groupBoxMenu.Visible = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private void rbLama_CheckedChanged(object sender, EventArgs e)
        { 
            try
            {
                if (rbLama.Checked == true)
                {
                    cbPembeli.Visible = true;
                    tbNamaPembeli.Visible = false;
                    IsiPembeliLama();
                }
                
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private void btnTambah_Click(object sender, EventArgs e)
        {
            try
            {
                //ID NamaMenu Harga Qty
                dtMenu = new DataTable();
                int qty = Convert.ToInt32(nudJumlah.Value);
                sqlQuery = "select m.id_menu as `Menu ID`, nama_menu as `Menu Name`, harga_menu as `Harga` from menu m, jadwal_catering jc, jadwal j where m.menu_delete = 0 " +
                    "and m.id_menu = jc.id_menu and j.id_jadwal = jc.id_jadwal and j.bulan = '" + dtpKirim.Value.ToString("MM") + "' and j.tahun = '" + dtpKirim.Value.ToString("yyyy") + 
                    "' and (jc.tanggal like '%" + dtpKirim.Value.ToString("dd") + "%' or jc.tanggal = '32') and m.id_menu = '" + cbMenuList.SelectedValue.ToString() + "'";
                sqlAdapter = new MySqlDataAdapter(sqlQuery, fLogin.conn);
                sqlAdapter.Fill(dtMenu);

                bool yes = false;
                if (dgvDftPesan.Rows[0].Cells[0].Value != null)
                {
                    for (int i = 0; i < dgvDftPesan.Rows.Count - 1; i++)
                    {
                        if (dgvDftPesan.Rows[i].Cells[0].Value == cbMenuList.SelectedValue)
                        {
                            yes = true;
                            dgvDftPesan.Rows[i].Cells[3].Value = Convert.ToInt32(dgvDftPesan.Rows[i].Cells[3].Value) + Convert.ToInt32(nudJumlah.Value);
                            MessageBox.Show("Jumlah pesanan sudah diubah sudah di-Ubah");
                            HitungSubtotal();
                            HitungTotal();
                        }
                    }
                }
                
                if (yes == false)
                {
                    
                    this.dgvDftPesan.Rows.Add(cbMenuList.SelectedValue.ToString(), dtMenu.Rows[0][1], dtMenu.Rows[0][2], Convert.ToInt32(nudJumlah.Value));
                    
                    dgvDftPesan.Refresh();
                    HitungSubtotal();
                    HitungTotal();
                }
                if (dgvDftPesan.Rows.Count > 1)
                {
                    dtpKirim.Enabled = false;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private void rbBaru_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                cbPembeli.Visible = false;
                tbNamaPembeli.Visible = true;
                tbAlamat.Text = "";
                tbNoTelp.Text = "";
                GenIDPembeli();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private void dtpKirim_ValueChanged(object sender, EventArgs e)
        {
            try
            {
                IsiMenu();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private void cbPembeli_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (cbPembeli.SelectedIndex == -1)
                {
                    tbIDPembeli.Text = "";
                }
                else
                {
                    tbIDPembeli.Text = cbPembeli.SelectedValue.ToString();
                    dtpb = new DataTable();
                    sqlQuery = "select id_pembeli as `ID Pembeli`, concat(FIRST_NAME, ' ', LAST_NAME) as `Nama Pembeli`, alamat as `Alamat`, no_hp as `No HP`, poin as `Poin` " +
                        "from pembeli where id_pembeli = '" + cbPembeli.SelectedValue.ToString() + "'";
                    sqlAdapter = new MySqlDataAdapter(sqlQuery, fLogin.conn);
                    sqlAdapter.Fill(dtpb);
                    if (dtpb.Rows.Count != 0)
                    {
                        tbAlamat.Text = dtpb.Rows[0]["Alamat"].ToString();
                        tbNoTelp.Text = dtpb.Rows[0]["No HP"].ToString();
                        lblPoin.Text = dtpb.Rows[0]["Poin"].ToString();
                    }
                    else
                    {
                        cbPembeli.SelectedIndex = -1;
                        tbIDPembeli.Text = "";
                        tbAlamat.Text = "";
                        tbNoTelp.Text = "";
                        lblPoin.Text = "0";
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private void btnDelete_Click(object sender, EventArgs e)
        {
            try
            {
                if (dgvDftPesan.CurrentCell == null)
                {
                    MessageBox.Show("Belum ada data yang dipilih");
                }
                else
                {
                    int row = dgvDftPesan.CurrentCell.RowIndex;
                    dgvDftPesan.Rows.RemoveAt(row);
                    
                    dgvDftPesan.CurrentCell = null;
                    HitungSubtotal();
                    HitungTotal();
                }
                if (dgvDftPesan.Rows.Count < 2)
                {
                    dtpKirim.Enabled = true;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private void cbPakaiPoin_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                if (cbPakaiPoin.Checked == true)
                {
                    HitungTotalPakePoin();
                }
                else if (cbPakaiPoin.Checked == false)
                {
                    HitungTotal();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
            }
        }
        public string firstName;
        public string lastName;
        private void PisahNama()
        {
            string fullName = tbNamaPembeli.Text;
            var names = fullName.Split(' ');
            firstName = names[0];
            lastName = names[1];
        }
        private void btnBuatPesan_Click(object sender, EventArgs e)
        {
            try
            {
                if(dgvDftPesan.Rows.Count <= 1 || tbAlamat.Text == "" || tbIDPembeli.Text == "" || tbNoTelp.Text == "" || 
                    (tbNamaPembeli.Text == "" && cbPembeli.Text == "") || (rbBaru.Checked == false && rbLama.Checked == false) || (rbCOD.Checked == false && rbTransfer.Checked == false))
                {
                    MessageBox.Show("Data belum lengkap");
                }
                else
                {
                    var result = MessageBox.Show("Sudah yakin dengan pesanan Anda?", "Warning!", MessageBoxButtons.YesNo);
                    if (result == DialogResult.Yes)
                    {
                        DateTime today = DateTime.Today;

                        MySqlCommand sqlCommand = new MySqlCommand();
                        MySqlDataAdapter sqlDataAdapter = new MySqlDataAdapter();
                        DataTable dtTransaksi = new DataTable();
                        sqlQuery = "select id_tb as `ID` from transaksi_beli where id_tb like '%" + today.ToString("yyyyMMdd") + "%' ";
                        sqlCommand = new MySqlCommand(sqlQuery, fLogin.conn);
                        sqlDataAdapter = new MySqlDataAdapter(sqlCommand);
                        sqlDataAdapter.Fill(dtTransaksi);

                        if (rbBaru.Checked == true)
                        {
                            PisahNama();
                            sqlQuery = "insert into pembeli values('" + tbIDPembeli.Text + "', null,'" + firstName + "','" + lastName + "','" + tbNoTelp.Text + "','" + tbAlamat.Text + "','0','-','-','-','0')";
                            sqlCommand = new MySqlCommand(sqlQuery, fLogin.conn);
                            sqlCommand.ExecuteNonQuery();
                        }

                        if (rbTransfer.Checked == true)
                        {
                            sqlQuery = "insert into transaksi_beli values('" + tBoxIDTB.Text.ToString() + "', '" + tbIDPembeli.Text.ToString() + "', '" + today.ToString("yyyy-MM-dd") + "', '" + 
                                Convert.ToInt32(lblSubtotal.Text) + "', '" + Convert.ToInt32(lblPoin.Text) + "', '" + dtpKirim.Value.ToString("yyyy-MM-dd") + "', null, '" + tbAlamat.Text + "', 10000, '" + 
                                Convert.ToInt32(lblTotal.Text) + "', 'Transfer Bank', '" + Convert.ToInt32(lblTotal.Text) / 10 + "', 'Pending', 0)";
                            sqlCommand = new MySqlCommand(sqlQuery, fLogin.conn);
                            sqlCommand.ExecuteNonQuery();

                            for (int i = 0; i < dgvDftPesan.Rows.Count - 1; i++)
                            {
                                sqlQuery = "insert into detail_beli values('" + tBoxIDTB.Text.ToString() + "', '" + tbIDPembeli.Text.ToString() + "','" + dgvDftPesan.Rows[i].Cells[0].ToString() + "', '" + 
                                    dgvDftPesan.Rows[i].Cells[3] + "', '" + Convert.ToInt32(dgvDftPesan.Rows[i].Cells[2].Value) * Convert.ToInt32(dgvDftPesan.Rows[i].Cells[3].Value) + "', null, null, 0)";
                                sqlCommand = new MySqlCommand(sqlQuery, fLogin.conn);
                                sqlCommand.ExecuteNonQuery();
                            }
                        }
                        else if (rbCOD.Checked == true)
                        {
                            sqlQuery = "insert into transaksi_beli values('" + tBoxIDTB.Text.ToString() + "', '" + tbIDPembeli.Text.ToString() + "', '" + today.ToString("yyyy-MM-dd") + "', '" + 
                                Convert.ToInt32(lblSubtotal.Text) + "', '" + Convert.ToInt32(lblPoin.Text) + "', '" + dtpKirim.Value.ToString("yyyy-MM-dd") + "', null, '" + tbAlamat.Text + "', '10000', " +
                                "'" + Convert.ToInt32(lblTotal.Text) + "', 'COD', '" + Convert.ToInt32(lblTotal.Text) / 10 + "', 'Pending', 0)";
                            sqlCommand = new MySqlCommand(sqlQuery, fLogin.conn);
                            sqlCommand.ExecuteNonQuery();

                            for (int i = 0; i < dgvDftPesan.Rows.Count - 1; i++)
                            {
                                sqlQuery = "insert into detail_beli values('" + tBoxIDTB.Text.ToString() + "', '" + tbIDPembeli.Text.ToString() + "','" + dgvDftPesan.Rows[i].Cells[0].Value.ToString() + "', " +
                                    "'" + dgvDftPesan.Rows[i].Cells[3].Value.ToString() + "', '" + Convert.ToInt32(dgvDftPesan.Rows[i].Cells[2].Value) * Convert.ToInt32(dgvDftPesan.Rows[i].Cells[3].Value) + "', " +
                                    "null, null, 0)";
                                sqlCommand = new MySqlCommand(sqlQuery, fLogin.conn);
                                sqlCommand.ExecuteNonQuery();
                            }
                        }
                        else
                        {
                            MessageBox.Show("Silahkan pilih metode pembayaran terlebih dahulu!");
                        }

                        if (cbPakaiPoin.Checked == true)
                        {
                            sqlQuery = "update pembeli set poin = 0 where id_pembeli = '" + tbIDPembeli.Text.ToString() + "'";
                            sqlCommand = new MySqlCommand(sqlQuery, fLogin.conn);
                            sqlCommand.ExecuteNonQuery();

                            sqlQuery = "update pembeli set poin = '"+ Convert.ToInt32(lblTotal.Text) / 10 + "' where id_pembeli = '" + tbIDPembeli.Text.ToString() + "'";
                            sqlCommand = new MySqlCommand(sqlQuery, fLogin.conn);
                            sqlCommand.ExecuteNonQuery();
                        }

                        if (rbLama.Checked == true)
                        {
                            sqlQuery = "select id_pembeli as `ID Pembeli`, concat(FIRST_NAME, ' ', LAST_NAME) as `Nama Pembeli`, alamat as `Alamat`, no_hp as `No HP`, poin as `Poin` from pembeli " +
                            "where id_pembeli = '" + cbPembeli.SelectedValue.ToString() + "'";
                            sqlAdapter = new MySqlDataAdapter(sqlQuery, fLogin.conn);
                            sqlAdapter.Fill(dtpb);
                            if (tbAlamat.Text.ToString() != dtpb.Rows[0][2].ToString())
                            {
                                sqlQuery = "update pembeli set alamat = '" + tbAlamat.Text.ToString() + "' where id_pembeli = '" + tbIDPembeli.Text.ToString() + "'";
                                sqlCommand = new MySqlCommand(sqlQuery, fLogin.conn);
                                sqlCommand.ExecuteNonQuery();
                            }
                            if (tbNoTelp.Text.ToString() != dtpb.Rows[0][3].ToString())
                            {
                                sqlQuery = "update pembeli set no_hp = '" + tbNoTelp.Text.ToString() + "' where id_pembeli = '" + tbIDPembeli.Text.ToString() + "'";
                                sqlCommand = new MySqlCommand(sqlQuery, fLogin.conn);
                                sqlCommand.ExecuteNonQuery();
                            }
                        }
                    }
                    MessageBox.Show("Data sudah mashokk!!");
                    BersihFormTransaksi();
                }
            }
                
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }


        private void InputMenu()
        {
            try
            {
                panelMenu.Visible = true;
                rbMakanan.Enabled = true;
                rbMinuman.Enabled = true;
                tBoxMenu.Text = "";
                tBoxDesc.Text = "";
                tBoxHarga.Text = "";
                tBoxTanggal.Text = "";
                IsiBulan();
                cboxBulan.SelectedIndex = -1;
                cboxTahun.SelectedIndex = -1;
                pbMenu.Image = App_Perusahaan.Properties.Resources.dish_0;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private void EditMenu()
        {
            try
            {
                panelMenu.Visible = true;

                dtMenu = new DataTable();
                sqlQuery = "select m.id_menu as `ID`, nama_menu as `Nama Menu`, deskripsi_menu as `Deskripsi`, harga_menu as `Harga`, foto_menu as " +
                    "`Foto`, bulan as `Bulan`, tahun as `Tahun`, tanggal as `Tanggal` from menu m, jadwal j, jadwal_catering jc where m.id_menu = jc.id_menu and " +
                    "j.id_jadwal = jc.id_jadwal and m.id_menu = '" + tBoxID.Text.ToString() + "'";
                sqlAdapter = new MySqlDataAdapter(sqlQuery, fLogin.conn);
                sqlAdapter.Fill(dtMenu);

                if (dtMenu.Rows[0]["ID"].ToString().Substring(0, 3) == "MKN")
                {
                    rbMakanan.Checked = true;
                }
                else if (dtMenu.Rows[0]["ID"].ToString().Substring(0, 3) == "MNM")
                {
                    rbMinuman.Checked = true;
                }
                tBoxMenu.Text = dtMenu.Rows[0]["Nama Menu"].ToString();
                tBoxDesc.Text = dtMenu.Rows[0]["Deskripsi"].ToString();
                tBoxHarga.Text = dtMenu.Rows[0]["Harga"].ToString();
                IsiBulan();
                cboxBulan.Text = dtMenu.Rows[0]["Bulan"].ToString();
                cboxTahun.Text = dtMenu.Rows[0]["Tahun"].ToString();
                tBoxTanggal.Text = dtMenu.Rows[0]["Tanggal"].ToString();
                if (dtMenu.Rows[0]["Foto"] == null)
                {
                    pbMenu.Image = App_Perusahaan.Properties.Resources.dish_0;
                }
                else
                {
                    byte[] fotomenu = (byte[])dtMenu.Rows[0]["Foto"];
                    MemoryStream ms = new MemoryStream(fotomenu);
                    pbMenu.Image = Image.FromStream(ms);
                    sqlAdapter.Dispose();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private void ShowAllMenu()
        {
            try
            {
                dtMenu = new DataTable();
                sqlQuery = "select id_menu as `Menu ID`, nama_menu as `Menu Name`, harga_menu as `Harga` from menu m where menu_delete = 0";
                sqlAdapter = new MySqlDataAdapter(sqlQuery, fLogin.conn);
                sqlAdapter.Fill(dtMenu);
                dgvMenu.DataSource = dtMenu;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private void ShowDailyMenu()
        {
            try
            {
                dtMenu = new DataTable();
                sqlQuery = "select m.id_menu as `Menu ID`, nama_menu as `Menu Name`, harga_menu as `Harga` from menu m, jadwal_catering jc, jadwal j " +
                    "where m.menu_delete = 0 and m.id_menu = jc.id_menu and j.id_jadwal = jc.id_jadwal and j.bulan = '" + dtpMenu.Value.ToString("MM") + 
                    "' and j.tahun = '" + dtpMenu.Value.ToString("yyyy") + "' and (jc.tanggal like '%" + dtpMenu.Value.ToString("dd") + "%' or jc.tanggal = '32')";
                sqlAdapter = new MySqlDataAdapter(sqlQuery, fLogin.conn);
                sqlAdapter.Fill(dtMenu);
                dgvMenu.DataSource = dtMenu;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void IsiIdmkn()
        {
            try
            {
                MySqlCommand sqlCommand = new MySqlCommand();
                sqlCommand.Connection = fLogin.conn;
                sqlCommand.CommandType = CommandType.StoredProcedure;
                sqlCommand.CommandText = "genIDMakan";

                MySqlParameter par = new MySqlParameter();
                par.Direction = ParameterDirection.ReturnValue;
                par.MySqlDbType = MySqlDbType.VarChar;
                par.Size = 10;
                sqlCommand.Parameters.Add(par);

                sqlCommand.ExecuteNonQuery();
                tBoxID.Text = par.Value.ToString();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
               
            }
        }
        private void IsiIdmnm()
        {
            try
            {
                MySqlCommand sqlCommand = new MySqlCommand();
                sqlCommand.Connection = fLogin.conn;
                sqlCommand.CommandType = CommandType.StoredProcedure;
                sqlCommand.CommandText = "genIDMinum";

                MySqlParameter par = new MySqlParameter();
                par.Direction = ParameterDirection.ReturnValue;
                par.MySqlDbType = MySqlDbType.VarChar;
                par.Size = 10;
                sqlCommand.Parameters.Add(par);

                sqlCommand.ExecuteNonQuery();
                tBoxID.Text = par.Value.ToString();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
               
            }
        }
        
        private void IsiBulan()
        {
            try
            {
                dtBulan = new DataTable();
                sqlQuery = "select bulan as `Angka Bulan` from jadwal order by `Angka Bulan` asc";
                sqlAdapter = new MySqlDataAdapter(sqlQuery, fLogin.conn);
                sqlAdapter.Fill(dtBulan);
                cboxBulan.DataSource = dtBulan;
                cboxBulan.DisplayMember = "Angka Bulan";
                cboxBulan.ValueMember = "Angka Bulan";
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private void GenIDTB()
        {
            try
            {
                MySqlCommand sqlCommand = new MySqlCommand();
                sqlCommand.Connection = fLogin.conn;
                sqlCommand.CommandType = CommandType.StoredProcedure;
                sqlCommand.CommandText = "GenidTB";

                MySqlParameter par = new MySqlParameter();
                par.Direction = ParameterDirection.ReturnValue;
                par.MySqlDbType = MySqlDbType.VarChar;
                par.Size = 11;
                sqlCommand.Parameters.Add(par);

                sqlCommand.ExecuteNonQuery();
                tBoxIDTB.Text = par.Value.ToString();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
            }

        }
        private void IsiPembeliLama()
        {
            try
            {
                dtPembeliLama = new DataTable();
                sqlQuery = "select id_pembeli as `ID Pembeli`, concat(FIRST_NAME, ' ', LAST_NAME) as `Nama Pembeli`, alamat as `Alamat`, no_hp as `No HP` from pembeli";
                sqlAdapter = new MySqlDataAdapter(sqlQuery, fLogin.conn);
                sqlAdapter.Fill(dtPembeliLama);

                cbPembeli.DataSource = dtPembeliLama;
                cbPembeli.DisplayMember = "Nama Pembeli";
                cbPembeli.ValueMember = "ID Pembeli";
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private void GenIDPembeli()
        {
            try
            {
                MySqlCommand sqlCommand = new MySqlCommand();
                sqlCommand.Connection = fLogin.conn;
                sqlCommand.CommandType = CommandType.StoredProcedure;
                sqlCommand.CommandText = "GenIdPembeli";

                MySqlParameter par = new MySqlParameter();
                par.Direction = ParameterDirection.ReturnValue;
                par.MySqlDbType = MySqlDbType.VarChar;
                par.Size = 12;
                sqlCommand.Parameters.Add(par);

                sqlCommand.ExecuteNonQuery();
                tbIDPembeli.Text = par.Value.ToString();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
            }
        }
        private void IsiMenu()
        {
            try
            {

                dtMenuList.Clear();
                sqlQuery = "select m.id_menu as `Menu ID`, nama_menu as `Menu Name`, harga_menu as `Harga` from menu m, jadwal_catering jc, jadwal j " +
                    "where m.menu_delete = 0 and m.id_menu = jc.id_menu and j.id_jadwal = jc.id_jadwal and j.bulan = '" + dtpKirim.Value.ToString("MM") + "' " +
                    "and j.tahun = '" + dtpKirim.Value.ToString("yyyy") + "' and (jc.tanggal like '%" + dtpKirim.Value.ToString("dd") + "%' or jc.tanggal = '32')";
                sqlAdapter = new MySqlDataAdapter(sqlQuery, fLogin.conn);
                sqlAdapter.Fill(dtMenuList);
                cbMenuList.DataSource = dtMenuList;
                cbMenuList.DisplayMember = "Menu Name";
                cbMenuList.ValueMember = "Menu ID";
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
            }
            
        }
        private void IsiDt()
        {
            try
            {
                dgvDftPesan.Columns.Add("ID Menu", "ID Menu");
                dgvDftPesan.Columns.Add("Nama Menu", "Nama Menu");
                dgvDftPesan.Columns.Add("Harga", "Harga");
                dgvDftPesan.Columns.Add("Qty", "Qty");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
            }
        }
        private void HitungSubtotal()
        {
            try
            {
                subtotal = 0;
                if (dgvDftPesan.Rows.Count == 1)
                {
                    lblSubtotal.Text = "0";
                }
                else
                {
                    for (int i = 0; i < dgvDftPesan.Rows.Count - 1; i++)
                    {
                        subtotal += Convert.ToInt32(dgvDftPesan.Rows[i].Cells[2].Value) * Convert.ToInt32(dgvDftPesan.Rows[i].Cells[3].Value);
                        lblSubtotal.Text = subtotal.ToString();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
            }
        }
        private void HitungTotal()
        {
            try
            {
                total = 0;
                if (dgvDftPesan.Rows.Count == 1)
                {
                    lblTotal.Text ="10000";
                }
                else
                {
                    for (int i = 0; i < dgvDftPesan.Rows.Count - 1; i++)
                    {
                        total = Convert.ToInt32(lblSubtotal.Text) + 10000;
                        lblTotal.Text = total.ToString();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
            }
        }
        private void HitungTotalPakePoin()
        {
            try
            {
                total = 0;
                if (dgvDftPesan.Rows.Count == 1)
                {
                    lblTotal.Text = "10000";
                }
                else
                {
                    for (int i = 0; i < dgvDftPesan.Rows.Count - 1; i++)
                    {
                        total = Convert.ToInt32(lblSubtotal.Text) + 10000 - Convert.ToInt32(lblPoin.Text);
                        lblTotal.Text = total.ToString();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
            }
        }

        private void BersihFormTransaksi()
        {
            try
            {
                tbNamaPembeli.Text = "";
                cbPembeli.Text = "";
                tbAlamat.Text = "";
                tbNoTelp.Text = "";
                rbCOD.Checked = false;
                rbTransfer.Checked = false;
                dgvDftPesan.DataSource = "";
                lblPoin.Text = "0";
                lblSubtotal.Text = "0";
                lblTotal.Text = "0";
                cbPakaiPoin.Checked = false;
                nudJumlah.Value = 0;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);

            }
        }
        private void btnCancel_Click(object sender, EventArgs e)
        {
            BersihFormTransaksi();
        }

        private void isiDgvLaporan(int pilihan)
        {
            try
            {
                string query = "";
                if (pilihan == 0)
                {
                    query = "SELECT * FROM vPenjualanMenu";
                }
                else if (pilihan == 1)
                {
                    query = "SELECT * FROM Transaksi_Bulanan";
                }
                else if (pilihan == 2)
                {
                    query = "SELECT * FROM Transaksi_Harian";
                }
                else if (pilihan == 3)
                {
                    query = "SELECT * FROM Transaksi_Pembeli";
                }
                else if (pilihan == 4)
                {
                    string myValue = Interaction.InputBox("Silahkan masukkan tanggal transaksi", "PERHATIAN", "2021/06/04", 0, 0);
                    query = "CALL LaporanTransaksiByBulan('" + myValue + "')";
                }
                else if (pilihan == 5)
                {
                    string myValue = Interaction.InputBox("Silahkan masukkan tanggal transaksi", "PERHATIAN", "2021-06-04", 0, 0);
                    query = "CALL LaporanTransaksiByHari('" + myValue + "')";
                }
                else if (pilihan == 6)
                {
                    string myValue = Interaction.InputBox("Silahkan masukkan minimum jumlah transaksi pembeli", "PERHATIAN", "1", 0, 0);
                    query = "CALL TransaksiPembeliByJumlah('"+myValue+"')";
                }

                MySqlDataAdapter daLaporan = new MySqlDataAdapter(query, fLogin.conn);
                DataSet dsLaporan = new DataSet();
                daLaporan.Fill(dsLaporan);
                dgvLaporan.DataSource = dsLaporan.Tables[0];
                dgvLaporan.Refresh();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void buttonLaporan_Click(object sender, EventArgs e)
        {
            try
            {
                gBoxTransaksi.Visible = false;
                gBoxPesanan.Visible = false;
                gBoxLaporan.Visible = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void cBoxLaporan_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                isiDgvLaporan(cBoxLaporan.SelectedIndex);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}