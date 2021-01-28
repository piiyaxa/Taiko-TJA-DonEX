using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;
using System.Diagnostics;
using System.Drawing;
using SlimDX.DirectInput;
using FDK;
using System.Reflection;

namespace TJAPlayer3
{
	internal class CStageタイトル : CStage
	{
		// コンストラクタ

		public CStageタイトル()
		{
			base.eステージID = CStage.Eステージ.タイトル;
			base.b活性化してない = true;
			base.list子Activities.Add( this.actFIfromSetup = new CActFIFOWhite() );
			base.list子Activities.Add( this.actFI = new CActFIFOWhite() );
			base.list子Activities.Add( this.actFO = new CActFIFOWhite() );
		}


		// CStage 実装

		public override void On活性化()
		{
			Trace.TraceInformation( "タイトルステージを活性化します。" );
			Trace.Indent();
			try
			{
				for( int i = 0; i < 4; i++ )
				{
					this.ctキー反復用[ i ] = new CCounter( 0, 0, 0, TJAPlayer3.Timer );
				}
				this.ct上移動用 = new CCounter();
				this.ct下移動用 = new CCounter();
				this.ctカーソルフラッシュ用 = new CCounter();
				base.On活性化();
			}
			finally
			{
				Trace.TraceInformation( "タイトルステージの活性化を完了しました。" );
				Trace.Unindent();
			}
		}
		public override void On非活性化()
		{
			Trace.TraceInformation( "タイトルステージを非活性化します。" );
			Trace.Indent();
			try
			{
				for( int i = 0; i < 4; i++ )
				{
					this.ctキー反復用[ i ] = null;
				}
				this.ct上移動用 = null;
				this.ct下移動用 = null;
				this.ctカーソルフラッシュ用 = null;
			}
			finally
			{
				Trace.TraceInformation( "タイトルステージの非活性化を完了しました。" );
				Trace.Unindent();
			}
			base.On非活性化();
		}
		public override void OnManagedリソースの作成()
		{
			//if( !base.b活性化してない )
			//{
			//	this.tx背景 = CDTXMania.tテクスチャの生成( CSkin.Path(@"Graphics\2_background.png"));
			//	this.txメニュー = CDTXMania.tテクスチャの生成( CSkin.Path( @"Graphics\2_menu.png" ));
			//	base.OnManagedリソースの作成();
			//}
			this.ct点滅 = new CCounter(0, 100, 15, TJAPlayer3.Timer);
			this.ctbgm = new CCounter(0, 250, 15, TJAPlayer3.Timer);
			this.ct移動用 = new CCounter();
			this.ct操作説明 = new CCounter(0, 1000, 15, TJAPlayer3.Timer);
			this.ct待機用 = new CCounter();
			this.ct待機用後 = new CCounter();
		}
		public override void OnManagedリソースの解放()
		{
			//if( !base.b活性化してない )
			//{
			//	CDTXMania.tテクスチャの解放( ref this.tx背景 );
			//	CDTXMania.tテクスチャの解放( ref this.txメニュー );
			//	base.OnManagedリソースの解放();
			//}
		}
		public override int On進行描画()
		{
			if( !base.b活性化してない )
			{
				#region [ 初めての進行描画 ]
				//---------------------
				if( base.b初めての進行描画 )
				{
					if( TJAPlayer3.r直前のステージ == TJAPlayer3.stage起動 )
					{
						this.actFIfromSetup.tフェードイン開始();
						base.eフェーズID = CStage.Eフェーズ.タイトル_起動画面からのフェードイン;
					}
					else
					{
						this.actFI.tフェードイン開始();
						base.eフェーズID = CStage.Eフェーズ.共通_フェードイン;
					}
					this.ctカーソルフラッシュ用.t開始( 0, 700, 5, TJAPlayer3.Timer );
					this.ctカーソルフラッシュ用.n現在の値 = 100;
					base.b初めての進行描画 = false;
                }
				//---------------------
				#endregion

				// 進行

				#region [ カーソル上移動 ]
				//---------------------
				if( this.ct上移動用.b進行中 )
				{
					this.ct上移動用.t進行();
					if( this.ct上移動用.b終了値に達した )
					{
						this.ct上移動用.t停止();
					}
				}
				//---------------------
				#endregion
				#region [ カーソル下移動 ]
				//---------------------
				if( this.ct下移動用.b進行中 )
				{
					this.ct下移動用.t進行();
					if( this.ct下移動用.b終了値に達した )
					{
						this.ct下移動用.t停止();
					}
				}
				//---------------------
				#endregion
				#region [ カーソルフラッシュ ]
				//---------------------
				this.ctカーソルフラッシュ用.t進行Loop();
				//---------------------
				#endregion

				

                // 描画

                if (TJAPlayer3.Tx.Title_Background != null )
                    TJAPlayer3.Tx.Title_Background.t2D描画( TJAPlayer3.app.Device, 0, 0 );
				if(this.ctbgm.n現在の値==249)
                {
					TJAPlayer3.Skin.bgmたたいてスタート.t再生する();
				}
				if (this.blstage==false)
				{
					if (TJAPlayer3.Tx.Title_PRESS != null)
						TJAPlayer3.Tx.Title_PRESS.t2D描画(TJAPlayer3.app.Device, 0, 0);
					if (TJAPlayer3.Input管理.Keyboard.bキーが押された((int)Key.Return))
                    {
						this.blstage = true;
						this.ct移動用.t開始(0, 100, 2, TJAPlayer3.Timer);
						this.ctbgm.n現在の値 = 0;
						this.ctbgm.t停止();
						TJAPlayer3.Skin.bgmたたいてスタート.t停止する();
					}
					if (TJAPlayer3.Input管理.Keyboard.bキーが押された((int)Key.P))
                    {
						this.blpass = true;
						this.ct移動用.t開始(0, 100, 2, TJAPlayer3.Timer);
						this.ct待機用.t開始(0, 100, 10, TJAPlayer3.Timer);
						this.ctbgm.n現在の値 = 0;
						this.ctbgm.t停止();
						TJAPlayer3.Skin.bgmたたいてスタート.t停止する();
					}
				}
				if (this.blstage==true)
                {
					if (TJAPlayer3.Tx.Title_Background != null)
						TJAPlayer3.Tx.Title_Background.t2D描画(TJAPlayer3.app.Device, 0, 0);

					if (TJAPlayer3.Tx.NamePlate[0] != null)
					{
						TJAPlayer3.Tx.NamePlate[0].t2D描画(TJAPlayer3.app.Device, TJAPlayer3.Skin.SongSelect_NamePlate_X[0]- 100 + this.ct移動用.n現在の値, TJAPlayer3.Skin.SongSelect_NamePlate_Y[0]);
					}
					// キー入力

					if (base.eフェーズID == CStage.Eフェーズ.共通_通常状態        // 通常状態、かつ
						&& TJAPlayer3.act現在入力を占有中のプラグイン == null)    // プラグインの入力占有がない
					{
						if (TJAPlayer3.Input管理.Keyboard.bキーが押された((int)Key.Escape))
							return (int)E戻り値.EXIT;

						this.ctキー反復用.Up.tキー反復(TJAPlayer3.Input管理.Keyboard.bキーが押されている((int)SlimDX.DirectInput.Key.UpArrow), new CCounter.DGキー処理(this.tカーソルを上へ移動する));
						this.ctキー反復用.R.tキー反復(TJAPlayer3.Pad.b押されているGB(Eパッド.HH), new CCounter.DGキー処理(this.tカーソルを上へ移動する));
						if (TJAPlayer3.Pad.b押された(E楽器パート.DRUMS, Eパッド.SD))
							this.tカーソルを上へ移動する();

						this.ctキー反復用.Down.tキー反復(TJAPlayer3.Input管理.Keyboard.bキーが押されている((int)SlimDX.DirectInput.Key.DownArrow), new CCounter.DGキー処理(this.tカーソルを下へ移動する));
						this.ctキー反復用.B.tキー反復(TJAPlayer3.Pad.b押されているGB(Eパッド.BD), new CCounter.DGキー処理(this.tカーソルを下へ移動する));
						if (TJAPlayer3.Pad.b押された(E楽器パート.DRUMS, Eパッド.LT))
							this.tカーソルを下へ移動する();

						if (TJAPlayer3.Tx.Title_Menu != null)
						{
							int x = MENU_X;
							int y = MENU_Y + (this.n現在のカーソル行 * MENU_H);
							if (this.ct上移動用.b進行中)
							{
								y += (int)((double)MENU_H / 2 * (Math.Cos(Math.PI * (((double)this.ct上移動用.n現在の値) / 100.0)) + 1.0));
							}
							else if (this.ct下移動用.b進行中)
							{
								y -= (int)((double)MENU_H / 2 * (Math.Cos(Math.PI * (((double)this.ct下移動用.n現在の値) / 100.0)) + 1.0));
							}
							if (this.ctカーソルフラッシュ用.n現在の値 <= 100)
							{
								float nMag = (float)(1.0 + ((((double)this.ctカーソルフラッシュ用.n現在の値) / 100.0) * 0.5));
								TJAPlayer3.Tx.Title_Menu.vc拡大縮小倍率.X = nMag;
								TJAPlayer3.Tx.Title_Menu.vc拡大縮小倍率.Y = nMag;
								TJAPlayer3.Tx.Title_Menu.Opacity = (int)(255.0 * (1.0 - (((double)this.ctカーソルフラッシュ用.n現在の値) / 100.0)));
								int x_magnified = x + ((int)((MENU_W * (1.0 - nMag)) / 2.0));
								int y_magnified = y + ((int)((MENU_H * (1.0 - nMag)) / 2.0));
								TJAPlayer3.Tx.Title_Menu.t2D描画(TJAPlayer3.app.Device, x_magnified, y_magnified, new Rectangle(0, MENU_H * 5, MENU_W, MENU_H));
							}
							TJAPlayer3.Tx.Title_Menu.vc拡大縮小倍率.X = 1f;
							TJAPlayer3.Tx.Title_Menu.vc拡大縮小倍率.Y = 1f;
							TJAPlayer3.Tx.Title_Menu.Opacity = 0xff;
							TJAPlayer3.Tx.Title_Menu.t2D描画(TJAPlayer3.app.Device, x, y, new Rectangle(0, MENU_H * 4, MENU_W, MENU_H));
						}
						if (TJAPlayer3.Tx.Title_Menu != null)
						{
							//this.txメニュー.t2D描画( CDTXMania.app.Device, 0xce, 0xcb, new Rectangle( 0, 0, MENU_W, MWNU_H ) );
							// #24525 2011.3.16 yyagi: "OPTION"を省いて描画。従来スキンとの互換性確保のため。
							TJAPlayer3.Tx.Title_Menu.t2D描画(TJAPlayer3.app.Device, MENU_X, MENU_Y, new Rectangle(0, 0, MENU_W, MENU_H));
							TJAPlayer3.Tx.Title_Menu.t2D描画(TJAPlayer3.app.Device, MENU_X, MENU_Y + MENU_H, new Rectangle(0, MENU_H * 2, MENU_W, MENU_H * 2));
						}
						if (this.ct移動用.b終了値に達した)
						{
							if ((TJAPlayer3.Pad.b押されたDGB(Eパッド.CY) || TJAPlayer3.Pad.b押された(E楽器パート.DRUMS, Eパッド.RD)) || (TJAPlayer3.Pad.b押された(E楽器パート.DRUMS, Eパッド.LC) || (TJAPlayer3.ConfigIni.bEnterがキー割り当てのどこにも使用されていない && TJAPlayer3.Input管理.Keyboard.bキーが押された((int)SlimDX.DirectInput.Key.Return))))
							{
								if ((this.n現在のカーソル行 == (int)E戻り値.GAMESTART - 1) && TJAPlayer3.Skin.soundゲーム開始音.b読み込み成功)
								{
									TJAPlayer3.Skin.soundゲーム開始音.t再生する();
								}
								else
								{
									TJAPlayer3.Skin.sound決定音.t再生する();
								}
								if (this.n現在のカーソル行 == (int)E戻り値.EXIT - 1)
								{
									return (int)E戻り値.EXIT;
								}
								this.actFO.tフェードアウト開始();
								base.eフェーズID = CStage.Eフェーズ.共通_フェードアウト;
							}
						}
					}
				}
				if (this.blpass == true)
                {
					if (TJAPlayer3.Tx.Title_pass_0 != null)
						TJAPlayer3.Tx.Title_pass_0.t2D描画(TJAPlayer3.app.Device, 0, 0);
					if(this.ct待機用.b終了値に達した)
                    {
						if (TJAPlayer3.Tx.Title_pass_1 != null)
							TJAPlayer3.Tx.Title_pass_1.t2D描画(TJAPlayer3.app.Device, 0, 0);
						this.ct待機用後.t進行();
						this.使えないんで = true;
					}
					if (this.ct待機用.n現在の値==99)
                    {
						TJAPlayer3.Skin.bgmぴこん.t再生する();
					}
					
				}
				if(this.ct待機用.n現在の値==99)
                {
					this.ct待機用後.t開始(0, 100, 20, TJAPlayer3.Timer);
				}
				if (this.使えないんで == true)
				{
					if (this.ct待機用後.b終了値に達した)
					{
						this.blpass = false;
						this.blpassok = true;
						this.ct待機用後.n現在の値 = 0;
					}
				}
				if(this.ct待機用後.n現在の値==99)
                {
					this.ct移動用.t開始(0, 100, 2, TJAPlayer3.Timer);
				}
				if(this.blpassok==true)
                {
					if (TJAPlayer3.Tx.Title_pass_2 != null)
						TJAPlayer3.Tx.Title_pass_2.t2D描画(TJAPlayer3.app.Device, 0, 0);
					if (TJAPlayer3.Input管理.Keyboard.bキーが押された((int)Key.K))
                    {
						this.blカーソル = true;
						TJAPlayer3.Skin.soundカーソル移動音.t再生する();
					}
					if (TJAPlayer3.Input管理.Keyboard.bキーが押された((int)Key.D))
					{
						this.blカーソル = false;
						TJAPlayer3.Skin.soundカーソル移動音.t再生する();
					}
					if (this.blカーソル == true)
					{
						if (TJAPlayer3.Tx.Title_no != null)
							TJAPlayer3.Tx.Title_no.t2D描画(TJAPlayer3.app.Device, 0, 0);
						if (TJAPlayer3.Input管理.Keyboard.bキーが押された((int)Key.F))
						{
							TJAPlayer3.Skin.sound決定音.t再生する();
							this.blpassok = false;
							
						}
						if (TJAPlayer3.Input管理.Keyboard.bキーが押された((int)Key.J))
						{
							TJAPlayer3.Skin.sound決定音.t再生する();
							this.blpassok = false;
							
						}
					}
					if (this.blカーソル == false)
					{
						if (TJAPlayer3.Tx.Title_yes != null)
							TJAPlayer3.Tx.Title_yes.t2D描画(TJAPlayer3.app.Device, 0, 0);
						if (TJAPlayer3.Input管理.Keyboard.bキーが押された((int)Key.F))
						{
							
							this.blstage = true;
							this.blpassok = false;
							TJAPlayer3.Skin.soundこれで始める.t再生する();
							this.ct移動用.t開始(0, 100, 2, TJAPlayer3.Timer);
						}
						if (TJAPlayer3.Input管理.Keyboard.bキーが押された((int)Key.J))
						{
							
							this.blstage = true;
							this.blpassok = false;
							TJAPlayer3.Skin.soundこれで始める.t再生する();
							this.ct移動用.t開始(0, 100, 2, TJAPlayer3.Timer);
						}
					}
				}
				if (TJAPlayer3.Tx.Title_base != null)
					TJAPlayer3.Tx.Title_base.t2D描画(TJAPlayer3.app.Device, 0, 0);
				if (this.don==true)
                {
					if (TJAPlayer3.Tx.Title_don != null)
						TJAPlayer3.Tx.Title_don.t2D描画(TJAPlayer3.app.Device, 0, 0);
				}
				if (this.ka == true)
				{
					if (TJAPlayer3.Tx.Title_ka != null)
						TJAPlayer3.Tx.Title_ka.t2D描画(TJAPlayer3.app.Device, 0, 0);
				}
				if (this.ct操作説明.n現在の値==0)
                {
					this.ka = false;
					this.don = true;
                }
				if (this.ct操作説明.n現在の値==499)
                {
					this.don = false;
					this.ka = true;
                }
				this.ct移動用.t進行();
				this.ct待機用.t進行();
				this.ctbgm.t進行Loop();
				this.ct操作説明.t進行Loop();
				this.ct点滅.t進行Loop();
				TJAPlayer3.Tx.NamePlate[0].Opacity = C変換.nParsentTo255((this.ct移動用.n現在の値 / 100.0));
				TJAPlayer3.Tx.Title_pass_2.Opacity = C変換.nParsentTo255((this.ct移動用.n現在の値 / 100.0));
				TJAPlayer3.Tx.Title_PRESS.Opacity = (int)(176.0 + 80.0 * Math.Sin((double)(2 * Math.PI * this.ct点滅.n現在の値 * 2 / 100.0)));
				TJAPlayer3.Tx.Title_don.Opacity = (int)(176.0 + 80.0 * Math.Sin((double)(2 * Math.PI * this.ct点滅.n現在の値 * 2 / 100.0)));
				TJAPlayer3.Tx.Title_ka.Opacity = (int)(176.0 + 80.0 * Math.Sin((double)(2 * Math.PI * this.ct点滅.n現在の値 * 2 / 100.0)));
				TJAPlayer3.Tx.Title_pass_0.Opacity = C変換.nParsentTo255((this.ct移動用.n現在の値 / 100.0));


				CStage.Eフェーズ eフェーズid = base.eフェーズID;
				switch( eフェーズid )
				{
					case CStage.Eフェーズ.共通_フェードイン:
						if( this.actFI.On進行描画() != 0 )
						{
							TJAPlayer3.Skin.soundタイトル音.t再生する();
							
							base.eフェーズID = CStage.Eフェーズ.共通_通常状態;
						}
						break;

					case CStage.Eフェーズ.共通_フェードアウト:
						if( this.actFO.On進行描画() == 0 )
						{
							break;
						}
						base.eフェーズID = CStage.Eフェーズ.共通_終了状態;
						switch ( this.n現在のカーソル行 )
						{
							case (int)E戻り値.GAMESTART - 1:
								return (int)E戻り値.GAMESTART;

							case (int) E戻り値.CONFIG - 1:
								return (int) E戻り値.CONFIG;

							case (int)E戻り値.EXIT - 1:
								return (int) E戻り値.EXIT;
								//return ( this.n現在のカーソル行 + 1 );
						}
						break;

					case CStage.Eフェーズ.タイトル_起動画面からのフェードイン:
						if( this.actFIfromSetup.On進行描画() != 0 )
						{
							TJAPlayer3.Skin.soundタイトル音.t再生する();
							
							base.eフェーズID = CStage.Eフェーズ.共通_通常状態;
						}
						break;
				}
			}
			return 0;
		}
		public enum E戻り値
		{
			継続 = 0,
			GAMESTART,
//			OPTION,
			CONFIG,
			EXIT
		}


		// その他

		#region [ private ]
		//-----------------
		[StructLayout( LayoutKind.Sequential )]
		private struct STキー反復用カウンタ
		{
			public CCounter Up;
			public CCounter Down;
			public CCounter R;
			public CCounter B;
			public CCounter this[ int index ]
			{
				get
				{
					switch( index )
					{
						case 0:
							return this.Up;

						case 1:
							return this.Down;

						case 2:
							return this.R;

						case 3:
							return this.B;
					}
					throw new IndexOutOfRangeException();
				}
				set
				{
					switch( index )
					{
						case 0:
							this.Up = value;
							return;

						case 1:
							this.Down = value;
							return;

						case 2:
							this.R = value;
							return;

						case 3:
							this.B = value;
							return;
					}
					throw new IndexOutOfRangeException();
				}
			}
		}
		private CCounter ct操作説明;
		private bool don;
		private bool ka;
		private CCounter ctbgm;
		private CCounter ct移動用;
		private CCounter ct待機用;
		private CCounter ct待機用後;
		private bool 使えないんで;
		private bool blpassok;
		private bool blカーソル;
		private bool blpass;
		private bool blstage;
		private CCounter ct点滅;
		private CActFIFOWhite actFI;
		private CActFIFOWhite actFIfromSetup;
		private CActFIFOWhite actFO;
		private CCounter ctカーソルフラッシュ用;
		private STキー反復用カウンタ ctキー反復用;
		private CCounter ct下移動用;
		private CCounter ct上移動用;
		private const int MENU_H = 39;
		private const int MENU_W = 227;
		private const int MENU_X = 506;
		private const int MENU_Y = 513;
		private int n現在のカーソル行;
	
		private void tカーソルを下へ移動する()
		{
			if ( this.n現在のカーソル行 != (int) E戻り値.EXIT - 1 )
			{
				TJAPlayer3.Skin.soundカーソル移動音.t再生する();
				this.n現在のカーソル行++;
				this.ct下移動用.t開始( 0, 100, 1, TJAPlayer3.Timer );
				if( this.ct上移動用.b進行中 )
				{
					this.ct下移動用.n現在の値 = 100 - this.ct上移動用.n現在の値;
					this.ct上移動用.t停止();
				}
			}
		}
		private void tカーソルを上へ移動する()
		{
			if ( this.n現在のカーソル行 != (int) E戻り値.GAMESTART - 1 )
			{
				TJAPlayer3.Skin.soundカーソル移動音.t再生する();
				this.n現在のカーソル行--;
				this.ct上移動用.t開始( 0, 100, 1, TJAPlayer3.Timer );
				if( this.ct下移動用.b進行中 )
				{
					this.ct上移動用.n現在の値 = 100 - this.ct下移動用.n現在の値;
					this.ct下移動用.t停止();
				}
			}
		}
		//-----------------
		#endregion
	}
}
