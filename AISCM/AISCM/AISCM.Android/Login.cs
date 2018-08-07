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
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using AISCM;
namespace AISCM.Droid
{
    [Activity(Label = "Login")]
    public class Login : Activity,GoogleApiClient.IConnectionCallbacks,GoogleApiClient.IOnConnectionFailedListener
    {
        private GoogleApiClient mGoogleApiClient;
        private SignInButton mGoogleSignIn;

        private Android.Gms.Common.ConnectionResult mConnectionResult;

        private bool mIntentInProgress;
        private bool mSignInClicked;
        private bool mInfoPopulated;
        public string user_email = "";
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Create your application here
            SetContentView(Resource.Layout.Login);
            mGoogleSignIn = FindViewById<SignInButton>(Resource.Id.sign_in_button);
            Android.Widget.Button b2 = FindViewById<Android.Widget.Button>(Resource.Id.button1);
            b2.Click += delegate
            {
                StartActivity(typeof(register));
            };

            mGoogleSignIn.Click += mGoogleSignIn_Click;

            GoogleApiClient.Builder builder = new GoogleApiClient.Builder(this);
            builder.AddConnectionCallbacks(this);
            builder.AddOnConnectionFailedListener(this);
            builder.AddApi(PlusClass.API);
            builder.AddScope(PlusClass.ScopePlusProfile);
            builder.AddScope(PlusClass.ScopePlusLogin);

            //Build our IGoogleApiClient
            mGoogleApiClient = builder.Build();
        }
        void mGoogleSignIn_Click(object sender, EventArgs e)
        {
            //Fire sign in
            if (!mGoogleApiClient.IsConnecting)
            {
                mSignInClicked = true;
                ResolveSignInError();
            }
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
                int id = Convert.ToInt32(agc.signin(email));
                MainPage mp = new MainPage();
                mp.user_type = id.ToString();
                mp.email = email;
                Global.email = email;
                MainActivity ma = new MainActivity();
                ma.email += email;
                Global.login = 1;
                Global_portable.email = email;
                user_email = email;
                Global_portable.user_id = id;
                switch (id)
                {
                    case -1:
                      
                        break;
                    case 0:
                        Toast.MakeText(this, "INVALID USER!!!", ToastLength.Long).Show();
                        break;
                    case 1:
                        Toast.MakeText(this, "Authenticated", ToastLength.Long);

                        StartActivity(typeof(MainActivity));
                        break;
                    case 2:
                        //StartActivity(typeof(farmer_home)); 
                        StartActivity(typeof(MainActivity));
                        //await Xamarin.Forms.Application.Current.MainPage.Navigation.PushModalAsync(new MainPage());
                        break;
                    
                }
                string location = plusUser.CurrentLocation;
                /* if (plusUser.HasDisplayName)
                 {
                     mName.Text += plusUser.DisplayName;
                 }

                 if (plusUser.HasTagline)
                 {
                     mTagline.Text += plusUser.Tagline;
                 }

                 if (plusUser.HasBraggingRights)
                 {
                     mBraggingRights.Text += plusUser.BraggingRights;

                 }
                 mEmail.Text += email;
                 if(plusUser.HasImage)
                 {
                     ImageView im = FindViewById<ImageView>(Resource.Id.imageView1);

                 }
                 if(plusUser.HasBirthday)
                 {
                     mBraggingRights.Text += plusUser.Birthday;
                 }

                 if (plusUser.HasRelationshipStatus)
                 {
                     switch (plusUser.RelationshipStatus)
                     {
                         case 0:
                             mRelationship.Text += "Single";
                             break;

                         case 1:
                             mRelationship.Text += "In a relationship";
                             break;

                         case 2:
                             mRelationship.Text += "Engaged";
                             break;

                         case 3:
                             mRelationship.Text += "Married";
                             break;

                         case 4:
                             mRelationship.Text += "It's complicated";
                             break;

                         case 5:
                             mRelationship.Text += "In an open relationship";
                             break;

                         case 6:
                             mRelationship.Text += "Widowed";
                             break;

                         case 7:
                             mRelationship.Text += "In a domestic partnership";
                             break;

                         case 8:
                             mRelationship.Text += "In a civil union";
                             break;

                         default:
                             mRelationship.Text += "Unknown";
                             break;
                     }
                 }

                 if (plusUser.HasGender)
                 {
                     switch (plusUser.Gender)
                     {
                         case 0:
                             mGender.Text += "Male";
                             break;

                         case 1:
                             mGender.Text += "Female";
                             break;

                         case 2:
                             mGender.Text += "Other";
                             break;

                         default:
                             mGender.Text += "Unknown";
                             break;
                     }
                 }*/

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