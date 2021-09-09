$(function () {

    //var skin = 'Snapgram';

    //var skins = {
    //    'Snapgram': {
    //        'avatars': true,
    //        'list': false,
    //        'autoFullScreen': false,
    //        'cubeEffect': true
    //    },

    //    'VemDeZAP': {
    //        'avatars': false,
    //        'list': true,
    //        'autoFullScreen': false,
    //        'cubeEffect': false
    //    },

    //    'FaceSnap': {
    //        'avatars': true,
    //        'list': false,
    //        'autoFullScreen': true,
    //        'cubeEffect': false
    //    },

    //    'Snapssenger': {
    //        'avatars': false,
    //        'list': false,
    //        'autoFullScreen': false,
    //        'cubeEffect': false
    //    }
    //};

    $.get("/ajax/stories", function (data) {
        var stories = [];

        for (var i = 0; i < data.length; i++) {
            var perfil = data[i];

            var items = [];
            for (var j = 0; j < perfil.stories.length; j++) {
                var item = perfil.stories[j];
                items.push(Zuck.buildItem(perfil.usuarioId + "-" + (j+1), item.type, item.length, item.src, item.preview, item.link, item.linkText, item.seen, item.time));
            }

            stories.push({
                id: perfil.usuarioId,
                photo: "/Images/perfil/" + perfil.usuarioId + "/1.jpg",
                name: perfil.name,
                link: "",
                lastUpdated: perfil.lastUpdated, //timestamp(perfil.dataStorie),
                items: items
            });
        }

        //console.log(Zuck);
     
        var s = new Zuck('stories', {
            backNative: false, //true, // false remove href # nas urls
            previousTap: true,
            autoFullScreen: false,
            skin: 'Snapgram',
            avatars: true,
            list: false,
            cubeEffect: false,
            localStorage: true,
            stories: stories
        });

    });

    //var timeIndex = 0;
    //var shifts = [35, 60, 60 * 3, 60 * 60 * 2, 60 * 60 * 25, 60 * 60 * 24 * 4, 60 * 60 * 24 * 10];
    //var timestamp = function () {
    //    var now = new Date();
    //    var shift = shifts[timeIndex++] || 0;
    //    var date = new Date(now - shift * 1000);

    //    return date.getTime() / 1000;
    //};


    //var stories = new Zuck('stories', {
    //    backNative: false, //true, // false remove href # nas urls
    //    previousTap: true,
    //    autoFullScreen: false,
    //    skin: 'Snapgram',
    //    avatars: true,
    //    list: false,
    //    cubeEffect: false,
    //    localStorage: true,
    //    stories: [
    //        {
    //            id: "9388cb84-21dc-4ad6-af29-0ac705ba49e3",
    //            photo: "/Images/perfil/9388cb84-21dc-4ad6-af29-0ac705ba49e3/1.jpg",
    //            name: "Sereia",
    //            link: "",
    //            lastUpdated: timestamp(),
    //            items: [
    //                Zuck.buildItem("9388cb84-21dc-4ad6-af29-0ac705ba49e3-1", "photo", 3, "/Scripts/plugin/stories/img/stories/1.jpg", "/Scripts/plugin/stories/img/stories/1.jpg", '', false, false, timestamp()),
    //                Zuck.buildItem("9388cb84-21dc-4ad6-af29-0ac705ba49e3-2", "video", 0, "/Scripts/plugin/stories/img/stories/2.mp4", "/Scripts/plugin/stories/img/stories/2.jpg", '', false, false, timestamp()),
    //                Zuck.buildItem("9388cb84-21dc-4ad6-af29-0ac705ba49e3-3", "photo", 3, "/Scripts/plugin/stories/img/stories/3.png", "/Scripts/plugin/stories/img/stories/3.png", 'https://poseydonrs.gear.host', 'Visite mew Site', false, timestamp())
    //            ]
    //        },
    //        {
    //            id: "gorillaz",
    //            photo: "/Scripts/plugin/stories/img/users/2.jpg",
    //            name: "Gorillaz",
    //            link: "",
    //            lastUpdated: timestamp(),
    //            items: [
    //                Zuck.buildItem("gorillaz-1", "video", 0, "/Scripts/plugin/stories/img/stories/4.mp4", "/Scripts/plugin/stories/img/stories/4.jpg", '', false, false, timestamp()),
    //                Zuck.buildItem("gorillaz-2", "photo", 3, "/Scripts/plugin/stories/img/stories/5.jpg", "/Scripts/plugin/stories/img/stories/5.jpg", '', false, false, timestamp()),
    //            ]
    //        },
    //        {
    //            id: "ladygaga",
    //            photo: "/Scripts/plugin/stories/img/users/3.jpg",
    //            name: "Lady Gaga",
    //            link: "",
    //            lastUpdated: timestamp(),
    //            items: [
    //                Zuck.buildItem("ladygaga-1", "photo", 5, "/Scripts/plugin/stories/img/stories/6.jpg", "/Scripts/plugin/stories/img/stories/6.jpg", '', false, false, timestamp()),
    //                Zuck.buildItem("ladygaga-2", "photo", 3, "/Scripts/plugin/stories/img/stories/7.jpg", "/Scripts/plugin/stories/img/stories/7.jpg", 'http://ladygaga.com', false, false, timestamp()),
    //            ]
    //        },
    //        {
    //            id: "starboy",
    //            photo: "/Scripts/plugin/stories/img/users/4.jpg",
    //            name: "The Weeknd",
    //            link: "",
    //            lastUpdated: timestamp(),
    //            items: [
    //                Zuck.buildItem("starboy-1", "photo", 5, "/Scripts/plugin/stories/img/stories/8.jpg", "/Scripts/plugin/stories/img/stories/8.jpg", '', false, false, timestamp())
    //            ]
    //        },
    //        {
    //            id: "riversquomo",
    //            photo: "/Scripts/plugin/stories/img/users/5.jpg",
    //            name: "Rivers Cuomo",
    //            link: "",
    //            lastUpdated: timestamp(),
    //            items: [
    //                Zuck.buildItem("riverscuomo", "photo", 10, "/Scripts/plugin/stories/img/stories/9.jpg", "/Scripts/plugin/stories/img/stories/9.jpg", '', false, false, timestamp())
    //            ]
    //        }
    //    ]
    //});

    //console.log($("#stories a").length);
    //$("#stories a").attr("href", "javascript:void(0);");

    //var stories = new Zuck({
    //    id: 'stories',                // timeline container id or reference
    //    skin: 'snapgram',      // container class
    //    avatars: true,         // shows user photo instead of last story item preview
    //    list: false,           // displays a timeline instead of carousel
    //    openEffect: true,      // enables effect when opening story - may decrease performance
    //    cubeEffect: false,     // enables the 3d cube effect when sliding story - may decrease performance
    //    autoFullScreen: false, // enables fullscreen on mobile browsers
    //    backButton: true,      // adds a back button to close the story viewer
    //    backNative: false,     // uses window history to enable back button on browsers/android
    //    previousTap: true,     // use 1/3 of the screen to navigate to previous item when tap the story

    //    stories: [             // array of stories
    //        // see stories structure example
    //    ],

    //    callbacks: {
    //        'onOpen': function (storyId, callback) { // on open story viewer
    //            callback();
    //        },

    //        'onView': function (storyId) { // on view story

    //        },

    //        'onEnd': function (storyId, callback) { // on end story
    //            callback();
    //        },

    //        'onClose': function (storyId, callback) { // on close story viewer
    //            callback();
    //        },

    //        'onNavigateItem': function (storyId, nextStoryId, callback) { // on navigate item of story
    //            callback();
    //        },
    //    },

    //    'language': { // if you need to translate :)
    //        'unmute': 'Touch to unmute',
    //        'keyboardTip': 'Press space to see next',
    //        'visitLink': 'Visit link',
    //        'time': {
    //            'ago': 'ago',
    //            'hour': 'hour',
    //            'hours': 'hours',
    //            'minute': 'minute',
    //            'minutes': 'minutes',
    //            'fromnow': 'from now',
    //            'seconds': 'seconds',
    //            'yesterday': 'yesterday',
    //            'tomorrow': 'tomorrow',
    //            'days': 'days'
    //        }
    //    }
    //});

});



/*
 
How to use
You can download this git repository or install via npm install zuck.js

Initialize:

var stories = new Zuck({
    id: '',                // timeline container id or reference
    skin: 'snapgram',      // container class
    avatars: true,         // shows user photo instead of last story item preview
    list: false,           // displays a timeline instead of carousel
    openEffect: true,      // enables effect when opening story - may decrease performance
    cubeEffect: false,     // enables the 3d cube effect when sliding story - may decrease performance
    autoFullScreen: false, // enables fullscreen on mobile browsers
    backButton: true,      // adds a back button to close the story viewer
    backNative: false,     // uses window history to enable back button on browsers/android
    previousTap: true,     // use 1/3 of the screen to navigate to previous item when tap the story

    stories: [             // array of stories
        // see stories structure example
    ],

    callbacks:  {
        'onOpen': function(storyId, callback) { // on open story viewer
            callback();
        },

        'onView': function(storyId) { // on view story

        },

        'onEnd': function(storyId, callback) { // on end story
            callback();
        },

        'onClose': function(storyId, callback) { // on close story viewer
            callback();
        },

        'onNavigateItem': function(storyId, nextStoryId, callback) { // on navigate item of story
            callback();
        },
    },

    'language': { // if you need to translate :)
      'unmute': 'Touch to unmute',
      'keyboardTip': 'Press space to see next',
      'visitLink': 'Visit link',
      'time': {
          'ago':'ago',
          'hour':'hour',
          'hours':'hours',
          'minute':'minute',
          'minutes':'minutes',
          'fromnow': 'from now',
          'seconds':'seconds',
          'yesterday': 'yesterday',
          'tomorrow': 'tomorrow',
          'days':'days'
      }
    }
});
Add/update a story:

stories.update({item object});
Remove a story:

stories.remove(storyId); // story id
Add/remove a story item:

stories.addItem(storyId, {item object});
stories.removeItem(storyId, itemId);
Stories structure example
A JSON example of the stories object:

{
    id: "",               // story id
    photo: "",            // story photo (or user photo)
    name: "",             // story name (or user name)
    link: "",             // story link (useless on story generated by script)
    lastUpdated: "",      // last updated date in unix time format
    seen: false,          // set true if user has opened - if local storage is used, you don't need to care about this

    items: [              // array of items
        // story item example
        {
            id: "",       // item id
            type: "",     // photo or video
            length: 3,    // photo timeout or video length in seconds - uses 3 seconds timeout for images if not set
            src: "",      // photo or video src
            preview: "",  // optional - item thumbnail to show in the story carousel instead of the story defined image
            link: "",     // a link to click on story
            linkText: "", // link text
            time: "",     // optional a date to display with the story item. unix timestamp are converted to "time ago" format
            seen: false   // set true if current user was read - if local storage is used, you don't need to care about this
        }
    ]
}
Alternate call
In your HTML:

<div id="stories">

    <!-- story -->
    <div class="story" data-id="{{story.id}}" data-last-updated="{{story.lastUpdated}}" data-photo="{{story.photo}}">
        <a href="{{story.link}}">
            <span><u style="background-image:url({{story.photo}});"></u><span>
            <span class="info">
                <strong>{{story.name}}</strong>
                <span class="time">{{story.lastUpdated}}</span>
            </span>
        </a>

        <ul class="items">

            <!-- story item -->
            <li data-id="{{story.items.id}}" data-time="{{story.items.time}}" class="{{story.items.seen}}">
                <a href="{{story.items.src}}" data-type="{{story.items.type}}" data-length="{{story.items.length}}" data-link="{{story.items.link}}" data-linkText="{{story.items.linkText}}">
                    <img src="{{story.items.preview}}">
                </a>
            </li>
            <!--/ story item -->

        </ul>
    </div>
    <!--/ story -->

</div>
Then in your JS:

var stories = new Zuck({{element id string or element reference}});
Tips
You can use with autoFullScreen option (disabled by default) to emulate an app on mobile devices.
If you use Ionic or some javascript that uses location.hash, you should always disable the "backNative" option which can mess up your navigation.
Limitations
On mobile browsers, video can't play with audio without a user gesture. So the script tries to play audio only when the user clicks to see the next story. When the story is playing automatically, the video is muted, but an alert is displayed so the user may click to turn the audio on.

Stories links opens in a new window too. This behaviour occurs because most websites are blocked on iframe embedding.

License
MIT
 
 */