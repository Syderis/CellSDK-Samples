/*
 * Copyright 2012 Syderis Technologies S.L. All rights reserved.
 * Use is subject to license terms.
 */

#region Using Statements
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization; 
#endregion

namespace ImageLoader.Beans
{
    [DataContract]
    public class GoogleResultBean
    {
        [DataMember]
        public ResponseDataType responseData;
    }

    [DataContract]
    public class ResponseDataType
    {
        [DataMember]
        public List<ResultType> results;
    }

    [DataContract]
    public class ResultType
    {
        [DataMember]
        public String url;
    }

    /* -----------------------------------
     * Example
     * -----------------------------------
       {
           "responseData":{
              "results":[
                 {
                    "GsearchResultClass":"GimageSearch",
                    "width":"960",
                    "height":"720",
                    "imageId":"ANd9GcScfaRfNwnKVM4bzoPe29Lb-vMxod5QbowDPRpUMCxUFugkSg6leh71tC4h",
                    "tbWidth":"148",
                    "tbHeight":"111",
                    "unescapedUrl":"http://openlab.jp/oscircular/ps3/Cell-BOF-HTTP-FUSE.jpg",
                    "url":"http://openlab.jp/oscircular/ps3/Cell-BOF-HTTP-FUSE.jpg",
                    "visibleUrl":"openlab.jp",
                    "title":"Cell-BOF-HTTP-FUSE.jpg",
                    "titleNoFormatting":"Cell-BOF-HTTP-FUSE.jpg",
                    "originalContextUrl":"http://openlab.jp/oscircular/ps3/index.html",
                    "content":"Based on Ubuntu 7.04 with \u003cb\u003eCELL\u003c/b\u003e",
                    "contentNoFormatting":"Based on Ubuntu 7.04 with CELL",
                    "tbUrl":"http://t2.gstatic.com/images?q\u003dtbn:ANd9GcScfaRfNwnKVM4bzoPe29Lb-vMxod5QbowDPRpUMCxUFugkSg6leh71tC4h"
                 },
                 {
                    "GsearchResultClass":"GimageSearch",
                    "width":"1152",
                    "height":"864",
                    "imageId":"ANd9GcTcpOljv7i9jr4zJmXKrgu6Ec2GDbTiTfz1R9OkUHJ3GO_vCyT4PSdDUEw",
                    "tbWidth":"150",
                    "tbHeight":"113",
                    "unescapedUrl":"http://johnnyjacob.files.wordpress.com/2009/07/03072009060.jpg",
                    "url":"http://johnnyjacob.files.wordpress.com/2009/07/03072009060.jpg",
                    "visibleUrl":"johnnyjacob.wordpress.com",
                    "title":"openSUSE 11.1 on PS3 « Johnny [Life \u0026amp; Code]",
                    "titleNoFormatting":"openSUSE 11.1 on PS3 « Johnny [Life \u0026amp; Code]",
                    "originalContextUrl":"http://johnnyjacob.wordpress.com/2009/07/04/opensuse-11-1-on-ps3/",
                    "content":"Now onto get the \u003cb\u003eCell SDK\u003c/b\u003e",
                    "contentNoFormatting":"Now onto get the Cell SDK",
                    "tbUrl":"http://t2.gstatic.com/images?q\u003dtbn:ANd9GcTcpOljv7i9jr4zJmXKrgu6Ec2GDbTiTfz1R9OkUHJ3GO_vCyT4PSdDUEw"
                 },
                 {
                    "GsearchResultClass":"GimageSearch",
                    "width":"720",
                    "height":"355",
                    "imageId":"ANd9GcR8PzkolGyV3dEMdKvoMhWTQwfEpYi7RpAB1Xb7TWYRZc36f1ecKp3EOkw",
                    "tbWidth":"140",
                    "tbHeight":"69",
                    "unescapedUrl":"http://www.ps3cluster.umassd.edu/images/mpirunpi.jpg",
                    "url":"http://www.ps3cluster.umassd.edu/images/mpirunpi.jpg",
                    "visibleUrl":"www.ps3cluster.umassd.edu",
                    "title":"PS3Cluster Guide : Step 3 MPI",
                    "titleNoFormatting":"PS3Cluster Guide : Step 3 MPI",
                    "originalContextUrl":"http://www.ps3cluster.umassd.edu/step3mpi.html",
                    "content":"and the \u003cb\u003eCell SDK\u003c/b\u003e",
                    "contentNoFormatting":"and the Cell SDK",
                    "tbUrl":"http://t0.gstatic.com/images?q\u003dtbn:ANd9GcR8PzkolGyV3dEMdKvoMhWTQwfEpYi7RpAB1Xb7TWYRZc36f1ecKp3EOkw"
                 },
              ],
              "cursor":{
                 "pages":[
                    {
                       "start":"0",
                       "label":1
                    },
                    {
                       "start":"5",
                       "label":2
                    },
                    {
                       "start":"10",
                       "label":3
                    },
                    {
                       "start":"15",
                       "label":4
                    },
                    {
                       "start":"20",
                       "label":5
                    },
                    {
                       "start":"25",
                       "label":6
                    },
                    {
                       "start":"30",
                       "label":7
                    },
                    {
                       "start":"35",
                       "label":8
                    }
                 ],
                 "estimatedResultCount":"2960",
                 "currentPageIndex":0,
                 "moreResultsUrl":"http://www.google.com/images?oe\u003dutf8\u0026ie\u003dutf8\u0026source\u003duds\u0026start\u003d0\u0026hl\u003des\u0026q\u003dCellSDK"
              }
           },
           "responseDetails":null,
           "responseStatus":200
        }
     */
}
