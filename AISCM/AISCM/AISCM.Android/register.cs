using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.Gms.Common.Apis;
using Android.Gms.Common;
using Android.Gms.Plus;
using Android.Gms.Plus.Model.People;
namespace AISCM.Droid
{
    [Activity(Label = "register")]
    public class register : Activity, GoogleApiClient.IConnectionCallbacks, GoogleApiClient.IOnConnectionFailedListener
    {
        private GoogleApiClient mGoogleApiClient;
        private SignInButton mGoogleSignIn;

        private Android.Gms.Common.ConnectionResult mConnectionResult;

        private bool mIntentInProgress;
        private bool mSignInClicked;
        private bool mInfoPopulated;
        public string user_email = "";
        public int user_type = 0;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.Register);
            Button register_farmer = FindViewById<Button>(Resource.Id.button1);
            Button register_muc = FindViewById<Button>(Resource.Id.button2);
            GoogleApiClient.Builder builder = new GoogleApiClient.Builder(this);
            builder.AddConnectionCallbacks(this);
            builder.AddOnConnectionFailedListener(this);
            builder.AddApi(PlusClass.API);
            builder.AddScope(PlusClass.ScopePlusProfile);
            builder.AddScope(PlusClass.ScopePlusLogin);

            //Build our IGoogleApiClient
            mGoogleApiClient = builder.Build();
            register_farmer.Click += delegate
            {
                user_type = 2;
                if (!mGoogleApiClient.IsConnecting)
                {
                    mSignInClicked = true;
                    ResolveSignInError();
                }
            };
            register_muc.Click += delegate
            {
                user_type = 3;
                if (!mGoogleApiClient.IsConnecting)
                {
                    mSignInClicked = true;
                    ResolveSignInError();
                }

            };

            
        }
        private void ResolveSignInError()
        {
            if (mGoogleApiClient.IsConnected)
            {
                Toast.MakeText(this, "Already Authenticated!!!!!!!!!!!!", ToastLength.Long).Show();
                //StartActivity(typeof(farmer_home));
                StartActivity(typeof(MainActivity));
                //No need to resolve errors, already connected
                return;

            }

            if (mConnectionResult.HasResolution)
            {
                try
                {
                    mIntentInProgress = true;
                    StartIntentSenderForResult(mConnectionResult.Resolution.IntentSender, 0, null, 0, 0, 0);
                }

                catch (Android.Content.IntentSender.SendIntentException e)
                {
                    //The intent was cancelled before it was sent. Return to the default
                    //state and attempt to connect to get an updated ConnectionResult
                    mIntentInProgress = false;
                    mGoogleApiClient.Connect();
                }
            }
        }

        protected override void OnActivityResult(int requestCode, Result resultCode, Intent data)
        {
            if (requestCode == 0)
            {
                if (resultCode != Result.Ok)
                {
                    mSignInClicked = false;
                }

                mIntentInProgress = false;

                if (!mGoogleApiClient.IsConnecting)
                {
                    mGoogleApiClient.Connect();
                }
            }
        }


        protected override void OnStart()
        {
            base.OnStart();
            mGoogleApiClient.Connect();
        }

        protected override void OnStop()
        {
            base.OnStop();
            if (mGoogleApiClient.IsConnected)
            {
                mGoogleApiClient.Disconnect();
            }
        }

        public async void OnConnected(Bundle connectionHint)
        {
            //Successful log in hooray!!
            mSignInClicked = false;

            if (mInfoPopulated)
            {
                //No need to populate info again
                return;
            }

            if (PlusClass.PeopleApi.GetCurrentPerson(mGoogleApiClient) != null)
            {
                IPerson plusUser = PlusClass.PeopleApi.GetCurrentPerson(mGoogleApiClient);
                var email = PlusClass.AccountApi.GetAccountName(mGoogleApiClient);
                net.azurewebsites.agc20171.AISCM agc = new net.azurewebsites.agc20171.AISCM();
                string name = plusUser.DisplayName.ToString();
                string gender = "";
                switch (plusUser.Gender)
                {
                    case 0:
                        gender += "Male";
                        break;

                    case 1:
                        gender += "Female";
                        break;

                    case 2:
                        gender += "Other";
                        break;

                    default:
                        gender += "Unknown";
                        break;
                }

                int c = agc.signup(email, name, gender,user_type.ToString());
                Global_portable.email = user_email;
                if (user_type == 2)
                    StartActivity(typeof(register_farmer));
                else
                    StartActivity(typeof(register_manu_company));
                mInfoPopulated = true;
            }
        }

        public void OnConnectionSuspended(int cause)
        {

        }

        public void OnConnectionFailed(ConnectionResult result)
        {
            if (!mIntentInProgress)
            {
                //Store the ConnectionResult so that we can use it later when the user clicks 'sign-in;
                mConnectionResult = result;

                if (mSignInClicked)
                {
                    //The user has already clicked 'sign-in' so we attempt to resolve all
                    //errors until the user is signed in, or the cancel
                    ResolveSignInError();
                }
            }
        }

        public void Authenticator_BrowsingCompleted(object sender, EventArgs e)
        {
            Toast.MakeText(this, "Brwosing successfully!!!!!!!!!!!!", ToastLength.Long).Show();
        }
    }
}