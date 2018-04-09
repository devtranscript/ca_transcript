using NReco.VideoConverter;
using NReco.VideoInfo;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.InteropServices;
using System.Text;

namespace ca_transcript
{
	class Program
	{
		static string str_path_ini;
		static string str_path_fin;
		static string str_Id;
		static string str_Title;
		static string str_Department;
		static string str_IsSealed;
		static string str_Exhibits;
		static string str_StartDate;
		static string str_EndDate;
		static string str_Location;
		static string str_Type;
		static string str_MasterGroups;
		static string str_nametable;
		static string str_id;
		static string xpath_value;
		static string xlocation_value;
		static string str_Path;
		static string str_DeviceId;
		static string str_Height;
		static string str_Width;
		static string str_Incomplete;
		static string str_Name;
		static string str_TimeStamp;
		static string str_Type_Event;
		static string str_TypeId;
		static string str_TypeCategoryId;
		static string str_IsSystemEvent;
		static string str_Isstatic;
		static string str_Identifier;
		static string str_EventNotes;
		static string dateStringstring;
		static string dateString;
		static string str_user;
		static string str_pass;
		static string str_IsPrivate;
		static int int_status;
		static int int_path;
		static int int_daysvideos;
		static int int_idrv;
		static Guid str_MasterId;
		static Guid str_SessionId;
		static Guid str_EventId;
		static Guid str_idcentro;
		static Guid str_idcontrol;
		static DateTime str_horario;
		static DateTime str_vhora;
		static DateTime str_fdate;
		static double differenceInMinutes;
		static string str_ffmpg;
		static void Main(string[] args)
		{

			ejecuta00();

			ejecuta000();

			ejecuta01();

			ejecuta02();


		}

		private static void ejecuta000()
		{
			foreach (Process p in Process.GetProcesses())
			{
				if (p.ProcessName == "ffmpeg")
				{
					str_ffmpg = p.ProcessName;
				}
				else
				{

				}

			}

			if (str_ffmpg == "ffmpeg")
			{


			}
			else
			{
				using (var edm_rutavideos = new db_transcriptEntities())
				{
					var i_rutavideos = (from c in edm_rutavideos.inf_ruta_videos
										select c).ToList();

					if (i_rutavideos.Count == 0)
					{
						Console.WriteLine("Sin caducidad de videos");
					}
					else
					{
						foreach (var item_rv in i_rutavideos)
						{
							str_pass = item_rv.ruta_pass_ini;
							str_user = item_rv.ruta_user_ini;
							str_path_fin = item_rv.desc_ruta_fin;
							str_path_ini = item_rv.desc_ruta_ini;
							int_idrv = item_rv.id_ruta_videos;

							try
							{
								using (new NetworkConnection(str_path_ini, new NetworkCredential(str_user, str_pass)))
								{
									using (var edm_material_err = new db_transcriptEntities())
									{
										var i_material_err = (from c in edm_material_err.inf_material
															  where c.id_estatus_material == 5
															  select c).ToList();

										if (i_material_err.Count == 0)
										{

										}
										else
										{

											using (var edm_material = new db_transcriptEntities())
											{
												var i_material = (from c in edm_material.inf_material
																  where c.id_estatus_material == 5
																  select c).ToList();

												foreach (var item in i_material)
												{
													string str_path_ini, str_path_fin;

													using (db_transcriptEntities data_path = new db_transcriptEntities())
													{
														var count_path = (from c in data_path.inf_ruta_videos
																		  select c).FirstOrDefault();

														str_path_fin = count_path.desc_ruta_fin;
														str_path_ini = count_path.desc_ruta_ini;
													}

													str_path_ini = str_path_fin + "\\" + item.archivo.ToString().Replace(".mp4", ".wmv");

													string str_file_save = str_path_ini.ToString();
													string str_save_file = str_path_ini.ToString().Replace(".wmv", ".mp4");
													var ffMpeg = new NReco.VideoConverter.FFMpegConverter();

													var ffProbe = new NReco.VideoInfo.FFProbe();
													var videoInfo = ffProbe.GetMediaInfo(str_file_save);

													string str_duration_wmv = videoInfo.Duration.Hours + ":" + videoInfo.Duration.Minutes + ":" + videoInfo.Duration.Seconds;

													try
													{

														using (var data_mat = new db_transcriptEntities())
														{
															var items_mat = (from c in data_mat.inf_material
																			 where c.sesion == item.sesion
																			 where c.duracion == str_duration_wmv
																			 select c).FirstOrDefault();

															items_mat.id_estatus_material = 6;

															data_mat.SaveChanges();
														}

														var two_user = new int?[] { 6 };

														ffMpeg.ConvertMedia(str_file_save, str_save_file, Format.mp4);


														using (var data_mat = new db_transcriptEntities())
														{
															var items_mat = (from c in data_mat.inf_material
																			 where c.sesion == item.sesion
																			 where c.duracion == str_duration_wmv
																			 select c).FirstOrDefault();

															items_mat.id_estatus_material = 1;

															data_mat.SaveChanges();
														}

														two_user = new int?[] { 1 };



													}
													catch
													{
														using (var data_mat = new db_transcriptEntities())
														{
															var items_mat = (from c in data_mat.inf_material
																			 where c.sesion == item.sesion
																			 where c.duracion == str_duration_wmv
																			 select c).FirstOrDefault();

															items_mat.id_estatus_material = 5;

															data_mat.SaveChanges();
														}


													}
													var videoInfo_mp4 = ffProbe.GetMediaInfo(str_file_save);

													string str_duration_mp4 = videoInfo_mp4.Duration.Hours + ":" + videoInfo_mp4.Duration.Minutes + ":" + videoInfo_mp4.Duration.Seconds;

													if (str_duration_wmv == str_duration_mp4)
													{
														File.Delete(str_file_save);

													}
													else
													{
														using (var data_mat = new db_transcriptEntities())
														{
															var items_mat = (from c in data_mat.inf_material
																			 where c.sesion == item.sesion
																			 where c.duracion == str_duration_wmv
																			 select c).FirstOrDefault();

															items_mat.id_estatus_material = 5;
															data_mat.SaveChanges();
														}
													}
												}
											}
										}
									}
								}
							}
							catch
							{
								Console.WriteLine("Error en ruta compartida");
							}
						}
					}
				}

			}
		}

		private static void ejecuta00()
		{
			foreach (Process p in Process.GetProcesses())
			{
				if (p.ProcessName == "ffmpeg")
				{
					str_ffmpg = p.ProcessName;
				}
				else
				{

				}

			}

			if (str_ffmpg == "ffmpeg")
			{

			}
			else
			{
				using (var edm_material = new db_transcriptEntities())
				{
					var i_material = (from c in edm_material.inf_material
									  where c.id_estatus_material == 6
									  select c).ToList();
					if (i_material.Count != 0)
					{

						foreach (var item in i_material)
						{
							str_path_fin = "C:\\inetpub\\wwwroot\\videos\\" + item.sesion;
							string str_file = "C:\\inetpub\\wwwroot\\videos\\" + item.sesion + ".jvl";
							string str_file_mp4 = "C:\\inetpub\\wwwroot\\videos\\" + item.archivo;
							string session = i_material[0].sesion;
							string archivo = i_material[0].archivo;

							Directory.Delete(str_path_fin, true);
							File.Delete(str_file);

							using (var edm_rutavideos = new db_transcriptEntities())
							{
								var i_rutavideos = (from c in edm_rutavideos.inf_ruta_videos
													select c).ToList();

								if (i_rutavideos.Count == 0)
								{
									Console.WriteLine("Sin ruta de videos");
								}
								else
								{
									str_pass = i_rutavideos[0].ruta_pass_ini;
									str_user = i_rutavideos[0].ruta_user_ini;
									str_path_fin = i_rutavideos[0].desc_ruta_fin;
									str_path_ini = i_rutavideos[0].desc_ruta_ini;

									Guid guid_idsessio;


									using (var edm_master = new tmp_javsEntities())
									{
										var i_master = (from c in edm_master.inf_Master
														where c.Id == session.ToString()
														select c).FirstOrDefault();

										guid_idsessio = i_master.id_control;


									}

									using (var edm_event = new tmp_javsEntities())
									{
										var i_event = (from c in edm_event.inf_Event
													   where c.id_control == guid_idsessio
													   select c).ToList();

										i_event.ForEach(c => edm_event.inf_Event.Remove(c));
										edm_event.SaveChanges();
									}

									using (var edm_media = new tmp_javsEntities())
									{
										var i_media = (from c in edm_media.inf_Media
													   where c.id_control == guid_idsessio
													   select c).ToList();

										i_media.ForEach(c => edm_media.inf_Media.Remove(c));
										edm_media.SaveChanges();
									}

									using (var edm_session = new tmp_javsEntities())
									{
										var i_session = (from c in edm_session.inf_Session
														 where c.id_control == guid_idsessio
														 select c).ToList();

										i_session.ForEach(c => edm_session.inf_Session.Remove(c));
										edm_session.SaveChanges();
									}

									using (var edm_master = new tmp_javsEntities())
									{
										var i_master = (from c in edm_master.inf_Master
														where c.id_control == guid_idsessio
														select c).ToList();

										i_master.ForEach(c => edm_master.inf_Master.Remove(c));
										edm_master.SaveChanges();
									}

									using (var edm_master = new db_transcriptEntities())
									{
										var i_master = (from c in edm_master.inf_material
														where c.id_control == guid_idsessio
														select c).ToList();

										i_master.ForEach(c => edm_master.inf_material.Remove(c));
										edm_master.SaveChanges();
									}

									using (new NetworkConnection(str_path_ini, new NetworkCredential(str_user, str_pass)))
									{
										using (var edm_material_err = new db_transcriptEntities())
										{
											var i_material_err = (from c in edm_material_err.inf_material
																  where c.id_estatus_material == 5
																  select c).ToList();

											if (i_material_err.Count == 0)
											{
												DirectoryInfo source = new DirectoryInfo(str_path_ini);
												DirectoryInfo target = new DirectoryInfo(str_path_fin);

												CopyFilesRecursively(source, target);
												DirectoryInfo directoryInfo = new DirectoryInfo(target.ToString());
												foreach (FileInfo file1 in directoryInfo.GetFiles("*.jvl"))
												{
													DataSet dataSet = new DataSet();
													int num = (int)dataSet.ReadXml(file1.FullName);
													DataTable dataTable1 = new DataTable();
													DataTable dataTable2 = new DataTable();
													DataTable dataTable3 = new DataTable();
													DataTable dataTable4 = new DataTable();

													DataTable table = dataSet.Tables["Master"];
													str_idcontrol = Guid.NewGuid();
													foreach (DataRow row in (InternalDataCollectionBase)table.Rows)
													{
														str_MasterId = Guid.Parse(row[0].ToString());
														str_Id = row[1].ToString();
														str_Title = row[2].ToString();
														str_Department = row[3].ToString();
														str_IsSealed = row[4].ToString();
														str_Exhibits = row[6].ToString();

														using (var edm_master = new tmp_javsEntities())
														{
															var i_master = (from c in edm_master.inf_Master
																			where c.MasterId == str_MasterId
																			select c).ToList();

															if (i_master.Count == 0)
															{
																using (tmp_javsEntities tmpJavsEntities = new tmp_javsEntities())
																{
																	inf_Master infMaster = new inf_Master()
																	{
																		id_control = str_idcontrol,
																		MasterId = new Guid?(str_MasterId),
																		Id = str_Id,
																		Title = str_Title,
																		Department = str_Department,
																		IsSealed = str_IsSealed,
																		Exhibits = str_Exhibits,
																		id_estatus = new int?(1),
																		fecha_registro = new DateTime?(DateTime.Now),
																		id_centro = new Guid?(str_idcentro)
																	};
																	tmpJavsEntities.inf_Master.Add(infMaster);
																	tmpJavsEntities.SaveChanges();
																}
															}
														}
													}
													foreach (DataRow row in (InternalDataCollectionBase)dataSet.Tables["Session"].Rows)
													{
														str_SessionId = Guid.Parse(row[0].ToString());
														str_StartDate = row[1].ToString();
														str_EndDate = row[2].ToString();
														str_Location = row[3].ToString();
														str_Type = row[4].ToString();
														str_MasterGroups = row[6].ToString();

														using (var edm_session = new tmp_javsEntities())
														{
															var i_session = (from c in edm_session.inf_Session
																			 where c.SessionId == str_SessionId
																			 select c).ToList();

															if (i_session.Count == 0)
															{
																using (tmp_javsEntities tmpJavsEntities = new tmp_javsEntities())
																{
																	inf_Session infSession = new inf_Session()
																	{
																		SessionId = str_SessionId,
																		StartDate = str_StartDate,
																		EndDate = str_EndDate,
																		Location = str_Location,
																		Type = str_Type,
																		MasterGroups = str_MasterGroups,
																		id_control = str_idcontrol
																	};
																	tmpJavsEntities.inf_Session.Add(infSession);
																	tmpJavsEntities.SaveChanges();
																}
															}
														}
													}
													foreach (DataRow row in (InternalDataCollectionBase)dataSet.Tables["Event"].Rows)
													{
														str_EventId = Guid.Parse(row[0].ToString());
														str_Name = row[1].ToString();
														str_TimeStamp = row[2].ToString();
														str_Type_Event = row[3].ToString();
														str_TypeId = row[4].ToString();
														str_TypeCategoryId = row[5].ToString();
														str_IsSystemEvent = row[6].ToString();
														str_IsPrivate = row[7].ToString();
														str_Identifier = row[8].ToString();
														str_EventNotes = row[9].ToString();

														using (var edm_event = new tmp_javsEntities())
														{
															var i_event = (from c in edm_event.inf_Event
																		   where c.EventId == str_EventId
																		   select c).ToList();

															if (i_event.Count == 0)
															{
																using (tmp_javsEntities tmpJavsEntities = new tmp_javsEntities())
																{
																	inf_Event infEvent = new inf_Event()
																	{
																		EventId = str_EventId,
																		Name = str_Name,
																		TimeStamp = str_TimeStamp,
																		Type = str_Type_Event,
																		TypeId = str_TypeId,
																		TypeCategoryId = str_TypeCategoryId,
																		IsSystemEvent = str_IsSystemEvent,
																		IsPrivate = str_IsPrivate,
																		Identifier = str_Identifier,
																		EventNotes = str_EventNotes,
																		id_control = str_idcontrol
																	};
																	tmpJavsEntities.inf_Event.Add(infEvent);
																	tmpJavsEntities.SaveChanges();
																}
															}
														}
													}
													foreach (DataRow row in (InternalDataCollectionBase)dataSet.Tables["Media"].Rows)
													{
														str_Path = row[0].ToString();
														str_DeviceId = row[1].ToString();
														str_Height = row[2].ToString();
														str_Width = row[3].ToString();
														str_Incomplete = row[4].ToString();

														using (var edm_media = new tmp_javsEntities())
														{
															var i_media = (from c in edm_media.inf_Media
																		   where c.Path == str_Path
																		   select c).ToList();

															if (i_media.Count == 0)
															{
																using (tmp_javsEntities tmpJavsEntities = new tmp_javsEntities())
																{
																	inf_Media infMedia = new inf_Media()
																	{
																		Path = str_Path,
																		DeviceId = str_DeviceId,
																		Height = str_Height,
																		Width = str_Width,
																		Incomplete = str_Incomplete,
																		id_control = str_idcontrol
																	};
																	tmpJavsEntities.inf_Media.Add(infMedia);
																	tmpJavsEntities.SaveChanges();
																}
																foreach (DirectoryInfo directory in directoryInfo.GetDirectories())
																{
																	string str1 = str_Path.Replace(".\\", "");
																	if (directory.Name == str1)
																	{
																		foreach (FileInfo file2 in new DirectoryInfo(directory.FullName).GetFiles("*.wmv"))
																		{
																			string outputFile = file2.FullName.Replace(".wmv", ".mp4");
																			string str2 = file2.Name.Replace(".wmv", ".mp4");
																			FFMpegConverter ffMpegConverter = new FFMpegConverter();
																			FFProbe ffProbe = new FFProbe();


																			var videoInfo = ffProbe.GetMediaInfo(file2.FullName);

																			string str_duration_wmv = videoInfo.Duration.Hours + ":" + videoInfo.Duration.Minutes + ":" + videoInfo.Duration.Seconds;

																			try
																			{
																				using (db_transcriptEntities transcriptEntities = new db_transcriptEntities())
																				{
																					inf_material infMaterial = new inf_material()
																					{
																						sesion = str_Id,
																						titulo = str_Title,
																						localizacion = str_Location,
																						tipo = str_Type,
																						archivo = str1 + "\\" + str2,
																						duracion = str_duration_wmv,
																						fecha_registro = DateTime.Now,
																						id_estatus_material = 5,
																						id_estatus_qa = 1,
																						id_ruta_videos = int_idrv,
																						id_control = new Guid?(str_idcontrol)
																					};
																					transcriptEntities.inf_material.Add(infMaterial);
																					transcriptEntities.SaveChanges();
																				}
																			}
																			catch
																			{


																			}

																		}
																	}
																}
															}
															else
															{

															}
														}
													}
												}
											}

										}
									}
								}
							}

							//if (Directory.Exists(str_path_fin))
							//{
							//    if (File.Exists(str_file_mp4))
							//    {

							//        try
							//        {
							//            FFProbe ffProbe = new FFProbe();
							//            var videoInfo = ffProbe.GetMediaInfo(str_file_mp4);
							//        }
							//        catch
							//        {


							//        }

							//    }

							//}
							//else
							//{
							//    Console.WriteLine("");
							//}


						}


					}
					else
					{
						Console.WriteLine("");
					}
				}
			}
		}

		private static void ejecuta02()
		{
			using (var edm_fechaconversion = new db_transcriptEntities())
			{
				var i_fechaconversion = (from c in edm_fechaconversion.inf_fecha_transformacion
										 select c).ToList();

				if (i_fechaconversion.Count == 0)
				{
					Console.WriteLine("Sin fecha de conversión");
				}
				else
				{

					str_horario = DateTime.Parse(i_fechaconversion[0].horario.ToString());
					str_vhora = DateTime.Now;
					differenceInMinutes = (double)(str_vhora - str_horario).Minutes;
					if (str_vhora >= str_horario && differenceInMinutes == 0.0)
					{
						using (var edm_rutavideos = new db_transcriptEntities())
						{
							var i_rutavideos = (from c in edm_rutavideos.inf_ruta_videos
												select c).ToList();

							if (i_rutavideos.Count == 0)
							{
								Console.WriteLine("Sin caducidad de videos");
							}
							else
							{
								foreach (var item_rv in i_rutavideos)
								{

									str_pass = item_rv.ruta_pass_ini;
									str_user = item_rv.ruta_user_ini;
									str_path_fin = item_rv.desc_ruta_fin;
									str_path_ini = item_rv.desc_ruta_ini;
									int_idrv = item_rv.id_ruta_videos;

									try
									{
										using (new NetworkConnection(str_path_ini, new NetworkCredential(str_user, str_pass)))
										{
											using (var edm_material_err = new db_transcriptEntities())
											{
												var i_material_err = (from c in edm_material_err.inf_material
																	  where c.id_estatus_material == 5
																	  select c).ToList();

												if (i_material_err.Count == 0)
												{
													DirectoryInfo source = new DirectoryInfo(str_path_ini);
													DirectoryInfo target = new DirectoryInfo(str_path_fin);
													if ((uint)Directory.GetFiles(target.ToString(), "*.mp4", SearchOption.AllDirectories).Length > 0U)
													{
														if (Directory.Exists(str_path_fin))
														{

														}
														else
														{

															Directory.CreateDirectory(str_path_fin);
														}


														CopyFilesRecursively(source, target);
														DirectoryInfo directoryInfo = new DirectoryInfo(target.ToString());
														foreach (FileInfo file1 in directoryInfo.GetFiles("*.jvl"))
														{
															DataSet dataSet = new DataSet();
															int num = (int)dataSet.ReadXml(file1.FullName);
															DataTable dataTable1 = new DataTable();
															DataTable dataTable2 = new DataTable();
															DataTable dataTable3 = new DataTable();
															DataTable dataTable4 = new DataTable();

															DataTable table = dataSet.Tables["Master"];
															str_idcontrol = Guid.NewGuid();

															foreach (DataRow row in (InternalDataCollectionBase)table.Rows)
															{
																str_MasterId = Guid.Parse(row[0].ToString());
																str_Id = row[1].ToString();
																str_Title = row[2].ToString();
																str_Department = row[3].ToString();
																str_IsSealed = row[4].ToString();
																str_Exhibits = row[6].ToString();

																using (var edm_master = new tmp_javsEntities())
																{
																	var i_master = (from c in edm_master.inf_Master
																					where c.MasterId == str_MasterId
																					select c).ToList();

																	if (i_master.Count == 0)
																	{
																		using (tmp_javsEntities tmpJavsEntities = new tmp_javsEntities())
																		{
																			inf_Master infMaster = new inf_Master()
																			{
																				id_control = str_idcontrol,
																				MasterId = new Guid?(str_MasterId),
																				Id = str_Id,
																				Title = str_Title,
																				Department = str_Department,
																				IsSealed = str_IsSealed,
																				Exhibits = str_Exhibits,
																				id_estatus = new int?(1),
																				fecha_registro = new DateTime?(DateTime.Now),
																				id_centro = new Guid?(str_idcentro)
																			};
																			tmpJavsEntities.inf_Master.Add(infMaster);
																			tmpJavsEntities.SaveChanges();
																		}
																	}
																}
															}
															foreach (DataRow row in (InternalDataCollectionBase)dataSet.Tables["Session"].Rows)
															{
																str_SessionId = Guid.Parse(row[0].ToString());
																str_StartDate = row[1].ToString();
																str_EndDate = row[2].ToString();
																str_Location = row[3].ToString();
																str_Type = row[4].ToString();
																str_MasterGroups = row[6].ToString();

																using (var edm_session = new tmp_javsEntities())
																{
																	var i_session = (from c in edm_session.inf_Session
																					 where c.SessionId == str_SessionId
																					 select c).ToList();

																	if (i_session.Count == 0)
																	{
																		using (tmp_javsEntities tmpJavsEntities = new tmp_javsEntities())
																		{
																			inf_Session infSession = new inf_Session()
																			{
																				SessionId = str_SessionId,
																				StartDate = str_StartDate,
																				EndDate = str_EndDate,
																				Location = str_Location,
																				Type = str_Type,
																				MasterGroups = str_MasterGroups,
																				id_control = str_idcontrol
																			};
																			tmpJavsEntities.inf_Session.Add(infSession);
																			tmpJavsEntities.SaveChanges();
																		}
																	}
																}
															}
															foreach (DataRow row in (InternalDataCollectionBase)dataSet.Tables["Event"].Rows)
															{
																str_EventId = Guid.Parse(row[0].ToString());
																str_Name = row[1].ToString();
																str_TimeStamp = row[2].ToString();
																str_Type_Event = row[3].ToString();
																str_TypeId = row[4].ToString();
																str_TypeCategoryId = row[5].ToString();
																str_IsSystemEvent = row[6].ToString();
																str_IsPrivate = row[7].ToString();
																str_Identifier = row[8].ToString();
																str_EventNotes = row[9].ToString();

																using (var edm_event = new tmp_javsEntities())
																{
																	var i_event = (from c in edm_event.inf_Event
																				   where c.EventId == str_EventId
																				   select c).ToList();

																	if (i_event.Count == 0)
																	{
																		using (tmp_javsEntities tmpJavsEntities = new tmp_javsEntities())
																		{
																			inf_Event infEvent = new inf_Event()
																			{
																				EventId = str_EventId,
																				Name = str_Name,
																				TimeStamp = str_TimeStamp,
																				Type = str_Type_Event,
																				TypeId = str_TypeId,
																				TypeCategoryId = str_TypeCategoryId,
																				IsSystemEvent = str_IsSystemEvent,
																				IsPrivate = str_IsPrivate,
																				Identifier = str_Identifier,
																				EventNotes = str_EventNotes,
																				id_control = str_idcontrol
																			};
																			tmpJavsEntities.inf_Event.Add(infEvent);
																			tmpJavsEntities.SaveChanges();
																		}
																	}
																}
															}
															foreach (DataRow row in (InternalDataCollectionBase)dataSet.Tables["Media"].Rows)
															{
																str_Path = row[0].ToString();
																str_DeviceId = row[1].ToString();
																str_Height = row[2].ToString();
																str_Width = row[3].ToString();
																str_Incomplete = row[4].ToString();

																using (var edm_media = new tmp_javsEntities())
																{
																	var i_media = (from c in edm_media.inf_Media
																				   where c.Path == str_Path
																				   select c).ToList();

																	if (i_media.Count == 0)
																	{
																		using (tmp_javsEntities tmpJavsEntities = new tmp_javsEntities())
																		{
																			inf_Media infMedia = new inf_Media()
																			{
																				Path = str_Path,
																				DeviceId = str_DeviceId,
																				Height = str_Height,
																				Width = str_Width,
																				Incomplete = str_Incomplete,
																				id_control = str_idcontrol
																			};
																			tmpJavsEntities.inf_Media.Add(infMedia);
																			tmpJavsEntities.SaveChanges();
																		}
																		foreach (DirectoryInfo directory in directoryInfo.GetDirectories())
																		{
																			string str1 = str_Path.Replace(".\\", "");
																			if (directory.Name == str1)
																			{
																				foreach (FileInfo file2 in new DirectoryInfo(directory.FullName).GetFiles("*.wmv"))
																				{
																					string outputFile = file2.FullName.Replace(".wmv", ".mp4");
																					string str2 = file2.Name.Replace(".wmv", ".mp4");
																					FFMpegConverter ffMpegConverter = new FFMpegConverter();
																					FFProbe ffProbe = new FFProbe();


																					var videoInfo = ffProbe.GetMediaInfo(file2.FullName);

																					string str_duration_wmv = videoInfo.Duration.Hours + ":" + videoInfo.Duration.Minutes + ":" + videoInfo.Duration.Seconds;

																					try
																					{
																						using (db_transcriptEntities transcriptEntities = new db_transcriptEntities())
																						{
																							inf_material infMaterial = new inf_material()
																							{
																								sesion = str_Id,
																								titulo = str_Title,
																								localizacion = str_Location,
																								tipo = str_Type,
																								archivo = str1 + "\\" + str2,
																								duracion = str_duration_wmv,
																								fecha_registro = DateTime.Now,
																								id_estatus_material = 6,
																								id_ruta_videos = int_idrv,
																								id_estatus_qa = 1,
																								id_control = new Guid?(str_idcontrol)
																							};
																							transcriptEntities.inf_material.Add(infMaterial);
																							transcriptEntities.SaveChanges();
																						}

																						ffMpegConverter.ConvertMedia(file2.FullName, outputFile, Format.mp4);


																						using (var data_mat = new db_transcriptEntities())
																						{
																							var items_mat = (from c in data_mat.inf_material
																											 where c.sesion == str_Id
																											 where c.duracion == str_duration_wmv
																											 select c).FirstOrDefault();

																							items_mat.id_estatus_material = 1;

																							data_mat.SaveChanges();
																						}

																					}
																					catch
																					{
																						using (var data_mat = new db_transcriptEntities())
																						{
																							var items_mat = (from c in data_mat.inf_material
																											 where c.sesion == str_Id
																											 where c.duracion == str_duration_wmv
																											 select c).FirstOrDefault();

																							items_mat.id_estatus_material = 5;

																							data_mat.SaveChanges();
																						}


																					}
																					var videoInfo_mp4 = ffProbe.GetMediaInfo(outputFile);

																					string str_duration_mp4 = videoInfo_mp4.Duration.Hours + ":" + videoInfo_mp4.Duration.Minutes + ":" + videoInfo_mp4.Duration.Seconds;

																					if (str_duration_wmv == str_duration_mp4)
																					{
																						File.Delete(file2.FullName);

																					}
																					else
																					{
																						using (var data_mat = new db_transcriptEntities())
																						{
																							var items_mat = (from c in data_mat.inf_material
																											 where c.sesion == str_Id
																											 where c.duracion == str_duration_wmv
																											 select c).FirstOrDefault();

																							items_mat.id_estatus_material = 5;
																							data_mat.SaveChanges();
																						}
																					}
																				}
																			}
																		}
																	}
																	else
																	{

																	}
																}
															}
														}
													}
													else
													{
														Directory.CreateDirectory(str_path_fin);

														CopyFilesRecursively(source, target);
														DirectoryInfo directoryInfo = new DirectoryInfo(target.ToString());
														foreach (FileInfo file1 in directoryInfo.GetFiles("*.jvl"))
														{
															DataSet dataSet = new DataSet();
															int num = (int)dataSet.ReadXml(file1.FullName);
															DataTable dataTable1 = new DataTable();
															DataTable dataTable2 = new DataTable();
															DataTable dataTable3 = new DataTable();
															DataTable dataTable4 = new DataTable();

															DataTable table = dataSet.Tables["Master"];
															str_idcontrol = Guid.NewGuid();

															foreach (DataRow row in (InternalDataCollectionBase)table.Rows)
															{
																str_MasterId = Guid.Parse(row[0].ToString());
																str_Id = row[1].ToString();
																str_Title = row[2].ToString();
																str_Department = row[3].ToString();
																str_IsSealed = row[4].ToString();
																str_Exhibits = row[6].ToString();

																using (var edm_master = new tmp_javsEntities())
																{
																	var i_master = (from c in edm_master.inf_Master
																					where c.MasterId == str_MasterId
																					select c).ToList();

																	if (i_master.Count == 0)
																	{
																		using (tmp_javsEntities tmpJavsEntities = new tmp_javsEntities())
																		{
																			inf_Master infMaster = new inf_Master()
																			{
																				id_control = str_idcontrol,
																				MasterId = new Guid?(str_MasterId),
																				Id = str_Id,
																				Title = str_Title,
																				Department = str_Department,
																				IsSealed = str_IsSealed,
																				Exhibits = str_Exhibits,
																				id_estatus = new int?(1),
																				fecha_registro = new DateTime?(DateTime.Now),
																				id_centro = new Guid?(str_idcentro)
																			};
																			tmpJavsEntities.inf_Master.Add(infMaster);
																			tmpJavsEntities.SaveChanges();
																		}
																	}
																}
															}
															foreach (DataRow row in (InternalDataCollectionBase)dataSet.Tables["Session"].Rows)
															{
																str_SessionId = Guid.Parse(row[0].ToString());
																str_StartDate = row[1].ToString();
																str_EndDate = row[2].ToString();
																str_Location = row[3].ToString();
																str_Type = row[4].ToString();
																str_MasterGroups = row[6].ToString();

																using (var edm_session = new tmp_javsEntities())
																{
																	var i_session = (from c in edm_session.inf_Session
																					 where c.SessionId == str_SessionId
																					 select c).ToList();

																	if (i_session.Count == 0)
																	{
																		using (tmp_javsEntities tmpJavsEntities = new tmp_javsEntities())
																		{
																			inf_Session infSession = new inf_Session()
																			{
																				SessionId = str_SessionId,
																				StartDate = str_StartDate,
																				EndDate = str_EndDate,
																				Location = str_Location,
																				Type = str_Type,
																				MasterGroups = str_MasterGroups,
																				id_control = str_idcontrol
																			};
																			tmpJavsEntities.inf_Session.Add(infSession);
																			tmpJavsEntities.SaveChanges();
																		}
																	}
																}
															}
															foreach (DataRow row in (InternalDataCollectionBase)dataSet.Tables["Event"].Rows)
															{
																str_EventId = Guid.Parse(row[0].ToString());
																str_Name = row[1].ToString();
																str_TimeStamp = row[2].ToString();
																str_Type_Event = row[3].ToString();
																str_TypeId = row[4].ToString();
																str_TypeCategoryId = row[5].ToString();
																str_IsSystemEvent = row[6].ToString();
																str_IsPrivate = row[7].ToString();
																str_Identifier = row[8].ToString();
																str_EventNotes = row[9].ToString();

																using (var edm_event = new tmp_javsEntities())
																{
																	var i_event = (from c in edm_event.inf_Event
																				   where c.EventId == str_EventId
																				   select c).ToList();

																	if (i_event.Count == 0)
																	{
																		using (tmp_javsEntities tmpJavsEntities = new tmp_javsEntities())
																		{
																			inf_Event infEvent = new inf_Event()
																			{
																				EventId = str_EventId,
																				Name = str_Name,
																				TimeStamp = str_TimeStamp,
																				Type = str_Type_Event,
																				TypeId = str_TypeId,
																				TypeCategoryId = str_TypeCategoryId,
																				IsSystemEvent = str_IsSystemEvent,
																				IsPrivate = str_IsPrivate,
																				Identifier = str_Identifier,
																				EventNotes = str_EventNotes,
																				id_control = str_idcontrol
																			};
																			tmpJavsEntities.inf_Event.Add(infEvent);
																			tmpJavsEntities.SaveChanges();
																		}
																	}
																}
															}
															foreach (DataRow row in (InternalDataCollectionBase)dataSet.Tables["Media"].Rows)
															{
																str_Path = row[0].ToString();
																str_DeviceId = row[1].ToString();
																str_Height = row[2].ToString();
																str_Width = row[3].ToString();
																str_Incomplete = row[4].ToString();

																using (var edm_media = new tmp_javsEntities())
																{
																	var i_media = (from c in edm_media.inf_Media
																				   where c.Path == str_Path
																				   select c).ToList();

																	if (i_media.Count == 0)
																	{
																		using (tmp_javsEntities tmpJavsEntities = new tmp_javsEntities())
																		{
																			inf_Media infMedia = new inf_Media()
																			{
																				Path = str_Path,
																				DeviceId = str_DeviceId,
																				Height = str_Height,
																				Width = str_Width,
																				Incomplete = str_Incomplete,
																				id_control = str_idcontrol
																			};
																			tmpJavsEntities.inf_Media.Add(infMedia);
																			tmpJavsEntities.SaveChanges();
																		}
																		foreach (DirectoryInfo directory in directoryInfo.GetDirectories())
																		{
																			string str1 = str_Path.Replace(".\\", "");
																			if (directory.Name == str1)
																			{
																				foreach (FileInfo file2 in new DirectoryInfo(directory.FullName).GetFiles("*.wmv"))
																				{
																					string outputFile = file2.FullName.Replace(".wmv", ".mp4");
																					string str2 = file2.Name.Replace(".wmv", ".mp4");
																					FFMpegConverter ffMpegConverter = new FFMpegConverter();
																					FFProbe ffProbe = new FFProbe();


																					var videoInfo = ffProbe.GetMediaInfo(file2.FullName);

																					string str_duration_wmv = videoInfo.Duration.Hours + ":" + videoInfo.Duration.Minutes + ":" + videoInfo.Duration.Seconds;

																					try
																					{
																						using (db_transcriptEntities transcriptEntities = new db_transcriptEntities())
																						{
																							inf_material infMaterial = new inf_material()
																							{
																								sesion = str_Id,
																								titulo = str_Title,
																								localizacion = str_Location,
																								tipo = str_Type,
																								archivo = str1 + "\\" + str2,
																								duracion = str_duration_wmv,
																								fecha_registro = DateTime.Now,
																								id_estatus_material = 6,
																								id_ruta_videos = int_idrv,
																								id_estatus_qa = 1,
																								id_control = new Guid?(str_idcontrol)
																							};
																							transcriptEntities.inf_material.Add(infMaterial);
																							transcriptEntities.SaveChanges();
																						}

																						ffMpegConverter.ConvertMedia(file2.FullName, outputFile, Format.mp4);


																						using (var data_mat = new db_transcriptEntities())
																						{
																							var items_mat = (from c in data_mat.inf_material
																											 where c.sesion == str_Id
																											 where c.duracion == str_duration_wmv
																											 select c).FirstOrDefault();

																							items_mat.id_estatus_material = 1;

																							data_mat.SaveChanges();
																						}
																					}
																					catch
																					{
																						using (var data_mat = new db_transcriptEntities())
																						{
																							var items_mat = (from c in data_mat.inf_material
																											 where c.sesion == str_Id
																											 where c.duracion == str_duration_wmv
																											 select c).FirstOrDefault();

																							items_mat.id_estatus_material = 5;

																							data_mat.SaveChanges();
																						}


																					}
																					var videoInfo_mp4 = ffProbe.GetMediaInfo(outputFile);

																					string str_duration_mp4 = videoInfo_mp4.Duration.Hours + ":" + videoInfo_mp4.Duration.Minutes + ":" + videoInfo_mp4.Duration.Seconds;

																					if (str_duration_wmv == str_duration_mp4)
																					{
																						File.Delete(file2.FullName);

																					}
																					else
																					{
																						using (var data_mat = new db_transcriptEntities())
																						{
																							var items_mat = (from c in data_mat.inf_material
																											 where c.sesion == str_Id
																											 where c.duracion == str_duration_wmv
																											 select c).FirstOrDefault();

																							items_mat.id_estatus_material = 5;
																							data_mat.SaveChanges();
																						}
																					}
																				}
																			}
																		}
																	}
																	else
																	{

																	}
																}
															}
														}
													}
												}
												else
												{

													using (var edm_material = new db_transcriptEntities())
													{
														var i_material = (from c in edm_material.inf_material
																		  where c.id_estatus_material == 6
																		  select c).ToList();

														foreach (var item in i_material)
														{
															string str_path_ini, str_path_fin;

															using (db_transcriptEntities data_path = new db_transcriptEntities())
															{
																var count_path = (from c in data_path.inf_ruta_videos
																				  select c).FirstOrDefault();

																str_path_fin = count_path.desc_ruta_fin;
																str_path_ini = count_path.desc_ruta_ini;
															}

															str_path_ini = str_path_fin + "\\" + item.archivo.ToString().Replace(".mp4", ".wmv");

															string str_file_save = str_path_ini.ToString();
															string str_save_file = str_path_ini.ToString().Replace(".wmv", ".mp4");
															var ffMpeg = new NReco.VideoConverter.FFMpegConverter();

															var ffProbe = new NReco.VideoInfo.FFProbe();
															var videoInfo = ffProbe.GetMediaInfo(str_file_save);

															string str_duration_wmv = videoInfo.Duration.Hours + ":" + videoInfo.Duration.Minutes + ":" + videoInfo.Duration.Seconds;

															try
															{

																using (var data_mat = new db_transcriptEntities())
																{
																	var items_mat = (from c in data_mat.inf_material
																					 where c.sesion == item.sesion
																					 where c.duracion == str_duration_wmv
																					 select c).FirstOrDefault();

																	items_mat.id_estatus_material = 6;

																	data_mat.SaveChanges();
																}

																var two_user = new int?[] { 6 };

																ffMpeg.ConvertMedia(str_file_save, str_save_file, Format.mp4);


																using (var data_mat = new db_transcriptEntities())
																{
																	var items_mat = (from c in data_mat.inf_material
																					 where c.sesion == item.sesion
																					 where c.duracion == str_duration_wmv
																					 select c).FirstOrDefault();

																	items_mat.id_estatus_material = 1;

																	data_mat.SaveChanges();
																}

																two_user = new int?[] { 1 };



															}
															catch
															{
																using (var data_mat = new db_transcriptEntities())
																{
																	var items_mat = (from c in data_mat.inf_material
																					 where c.sesion == item.sesion
																					 where c.duracion == str_duration_wmv
																					 select c).FirstOrDefault();

																	items_mat.id_estatus_material = 5;

																	data_mat.SaveChanges();
																}


															}
															var videoInfo_mp4 = ffProbe.GetMediaInfo(str_file_save);

															string str_duration_mp4 = videoInfo_mp4.Duration.Hours + ":" + videoInfo_mp4.Duration.Minutes + ":" + videoInfo_mp4.Duration.Seconds;

															if (str_duration_wmv == str_duration_mp4)
															{
																File.Delete(str_file_save);

															}
															else
															{
																using (var data_mat = new db_transcriptEntities())
																{
																	var items_mat = (from c in data_mat.inf_material
																					 where c.sesion == item.sesion
																					 where c.duracion == str_duration_wmv
																					 select c).FirstOrDefault();

																	items_mat.id_estatus_material = 5;
																	data_mat.SaveChanges();
																}
															}
														}
													}
												}
											}
										}
									}
									catch
									{
										Console.WriteLine("Error en ruta compartida");
									}
								}
							}
						}
					}
					else
					{
						Console.WriteLine("No es la hora para convertir");
					}
				}

			}
		}

		private static void ejecuta01()
		{
			using (var edm_rutavideos = new db_transcriptEntities())
			{
				var i_rutavideos = (from c in edm_rutavideos.inf_ruta_videos
									select c).ToList();
				if (i_rutavideos.Count == 0)
				{
					Console.WriteLine("Sin fecha de conversión");
				}
				else
				{
					using (var edm_caducidadvideos = new db_transcriptEntities())
					{
						var i_caducidadvideos = (from c in edm_caducidadvideos.inf_caducidad_videos
												 select c).ToList();
						if (i_caducidadvideos.Count == 0)
						{
							Console.WriteLine("Sin caducidad de videos");
						}
						else
						{
							str_path_fin = i_rutavideos[0].desc_ruta_fin;
							str_path_ini = i_rutavideos[0].desc_ruta_ini;

							if (Directory.Exists(str_path_fin))
							{
								int_daysvideos = int.Parse(i_caducidadvideos[0].dias_caducidad.ToString());



								using (var edm_material = new db_transcriptEntities())
								{
									var i_material = (from c in edm_material.inf_material
													  where c.id_estatus_material == 1
													  select c).ToList();
									if (i_material.Count != 0)
									{

										if (Directory.Exists(str_path_fin + "\\" + i_material[0].sesion))
										{
											//DateTime oldDate = DateTime.Parse(item.fecha_registro.ToString());
											DateTime oldDate = Directory.GetCreationTime(str_path_fin + "\\" + i_material[0].sesion);
											DateTime newDate = DateTime.Now;

											// Difference in days, hours, and minutes.
											TimeSpan ts = newDate - oldDate;

											// Difference in days.
											int differenceInDays = ts.Days;
											if (differenceInDays >= int_daysvideos)
											{
												foreach (var item in i_material)
												{
													string str_rutafile = str_path_fin + "\\" + item.archivo;
													File.Delete(str_rutafile);
													item.id_estatus_material = 4;
													edm_material.SaveChanges();
												}

												File.Delete(str_path_fin + "\\" + i_material[0].sesion + ".jvl");
												Console.WriteLine("Eliminado video");
												Directory.Delete(str_path_fin + "\\" + i_material[0].sesion);
												Console.WriteLine("Eliminada carpeta video");
											}
											else
											{
												Console.WriteLine("No es la hora para el borrado");
											}
										}
										else
										{
											Console.WriteLine("Sin carpeta para borrar");
										}

									}
									else
									{
										Console.WriteLine("Sin videos para borrar");
									}
								}
							}
							else
							{
								Directory.CreateDirectory(str_path_fin);
								Console.WriteLine("Creando carpeta videos");
							}
						}
					}
				}

			}
		}



		private static void CopyFilesRecursively(DirectoryInfo source, DirectoryInfo target)
		{
			FileLibrary.CopyDirectory(source.ToString(), target.ToString(), false);

		}
		public class NetworkConnection : IDisposable
		{
			private readonly string _networkName;

			public NetworkConnection(string networkName, NetworkCredential credentials)
			{
				_networkName = networkName;
				int error = NetworkConnection.WNetAddConnection2(new NetResource()
				{
					Scope = ResourceScope.GlobalNetwork,
					ResourceType = ResourceType.Disk,
					DisplayType = ResourceDisplaytype.Share,
					RemoteName = networkName.TrimEnd('\\')
				}, credentials.Password, credentials.UserName, 0);
				if ((uint)error > 0U)
					throw new Win32Exception(error);
			}

			public event EventHandler<EventArgs> Disposed;

			public void Dispose()
			{
				Dispose(true);
				GC.SuppressFinalize((object)this);
			}

			protected virtual void Dispose(bool disposing)
			{
				if (disposing)
				{
					// ISSUE: reference to a compiler-generated field
					EventHandler<EventArgs> disposed = Disposed;
					if (disposed != null)
						disposed((object)this, EventArgs.Empty);
				}
				NetworkConnection.WNetCancelConnection2(_networkName, 0, true);
			}

			[DllImport("mpr.dll")]
			private static extern int WNetAddConnection2(NetResource netResource, string password, string username, int flags);
			[DllImport("mpr.dll")]
			private static extern int WNetCancelConnection2(string name, int flags, bool force);

			~NetworkConnection()
			{
				Dispose(false);
			}
		}

		[StructLayout(LayoutKind.Sequential)]
		public class NetResource
		{
			public ResourceScope Scope;
			public ResourceType ResourceType;
			public ResourceDisplaytype DisplayType;
			public int Usage;
			public string LocalName;
			public string RemoteName;
			public string Comment;
			public string Provider;
		}

		public enum ResourceScope
		{
			Connected = 1,
			GlobalNetwork = 2,
			Remembered = 3,
			Recent = 4,
			Context = 5,
		}

		public enum ResourceType
		{
			Any = 0,
			Disk = 1,
			Print = 2,
			Reserved = 8,
		}

		public enum ResourceDisplaytype
		{
			Generic,
			Domain,
			Server,
			Share,
			File,
			Group,
			Network,
			Root,
			Shareadmin,
			Directory,
			Tree,
			Ndscontainer,
		}


	}
}
