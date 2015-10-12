import ddf.minim.analysis.*;
import ddf.minim.*;
import ddf.minim.signals.*;
import processing.video.*;
Capture cap;
Minim minim;
AudioOutput out;

// 各音の周波数設定
final int note[]={262,294,330,349,392,440,494,523};

void setup(){
  minim = new Minim(this);
  out = minim.getLineOut(Minim.STEREO);
  size(640, 480);                  // 画面サイズ
  smooth();                        // 描画を滑らかに
  String[] cams = Capture.list();       // 接続されている全カメラ名を取得
  cap = new Capture(this, cams[0]);  // カメラのキャプチャ
  cap.start();
  colorMode(HSB,note.length);
}

void draw() {
  if (cap.available()){
    cap.read();    // カメラ映像の取得
    image(cap, 0, 0);  // カメラ映像を画面に表示
    color c = get(320,240);  // 画面の座標(320,240)の画素値を抽出
    playColor(c);  // 抽出した画素値に応じて音を鳴らす
  }
}

// このメソッドに色を与えると対応した音を出す
void playColor(color h){
  MyNote newNote;
  newNote = new MyNote(note[(int)hue(h)], 0.2);
}


void stop(){
  out.close();
  minim.stop();
  super.stop();
}

class MyNote implements AudioSignal
{
  private float freq;
  private float level;
  private float alph;
  private SineWave sine;

  MyNote(float pitch, float amplitude){
  freq = pitch;
  level = amplitude;
  sine = new SineWave(freq, level, out.sampleRate());
  alph = 0.9; // Decay constant for the envelope
  out.addSignal(this);
}

  void updateLevel(){
    level = level * alph;
    sine.setAmp(level);
    if (level < 0.01) {
      out.removeSignal(this);
    }
  }

  void generate(float [] samp){
    sine.generate(samp);
    updateLevel();
  }

  void generate(float [] sampL, float [] sampR){
    sine.generate(sampL, sampR);
    updateLevel();
  }
}
