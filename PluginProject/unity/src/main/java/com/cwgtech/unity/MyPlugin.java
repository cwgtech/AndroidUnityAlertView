package com.cwgtech.unity;

import android.app.Activity;
import android.app.AlertDialog;
import android.content.DialogInterface;
import android.util.Log;

/**
 * Created by colin on 2/4/18.
 */

public class MyPlugin {
    private static final MyPlugin ourInstance = new MyPlugin();

    private static final String LOGTAG = "CWGTech";

    public static MyPlugin getInstance() {
        return ourInstance;
    }

    public static Activity mainActivity;

    public interface AlertViewCallback {
        public void onButtonTapped(int id);
    }
    private long startTime;

    private MyPlugin() {
        Log.i(LOGTAG,"Created MyPlugin");
        startTime = System.currentTimeMillis();
    }

    public double getElapsedTime()
    {
        return (System.currentTimeMillis()-startTime)/1000.0f;
    }

    public void showAlertView(String[] strings, final AlertViewCallback callback)
    {
        if (strings.length<3)
        {
            Log.i(LOGTAG,"Error - expected at least 3 strings, got " + strings.length);
            return;
        }
        DialogInterface.OnClickListener myClickListener = new DialogInterface.OnClickListener() {
            @Override
            public void onClick(DialogInterface dialogInterface, int id) {
                dialogInterface.dismiss();
                Log.i(LOGTAG, "Tapped: " + id);
                callback.onButtonTapped(id);
            }
        };

        AlertDialog alertDialog = new AlertDialog.Builder(mainActivity)
                .setTitle(strings[0])
                .setMessage(strings[1])
                .setCancelable(false)
                .create();
        alertDialog.setButton(AlertDialog.BUTTON_NEUTRAL,strings[2],myClickListener);
        if (strings.length>3)
            alertDialog.setButton(AlertDialog.BUTTON_NEGATIVE,strings[3],myClickListener);
        if (strings.length>4)
            alertDialog.setButton(AlertDialog.BUTTON_POSITIVE,strings[4],myClickListener);
        alertDialog.show();
    }
}
