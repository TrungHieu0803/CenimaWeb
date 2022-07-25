//const { signalR } = require("../microsoft/signalr/dist/browser/signalr");

$(() => {
    loadServicesData();
    var connection = new signalR.HubConnectionBuilder().withUrl('/hub').build();
    connection.start();

    connection.on("loadServices", function () {
        console.log(123123);
        loadServicesData();
    });

    loadServicesData();

    function loadServicesData() {
        var tr = "";

        $.ajax({
            url: '/Movies/Details/?handler=Rate',
            method: 'GET',
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: (result) => {
                
                //$.each(result, (k, v) => {
                //  //  console.log(v);
                //    tr +=   `<div class="card mb-4">
                //                <div class="card-body">
                //                    <p>${v}</p>

                //                    <div class="d-flex justify-content-between">
                //                        <div class="d-flex flex-row align-items-center">
                //                            <img src="https://mdbcdn.b-cdn.net/img/Photos/Avatars/img%20(4).webp" alt="avatar" width="25"
                //                             height="25" />
                //                            <p class="small mb-0 ms-2">@p.Fullname</p>
                //                        </div>
                //                        <div class="d-flex flex-row align-items-center">

                //                            <p class="small text-muted mb-0">@rate.NumericRating</p>
                //                        </div>
                //                    </div>
                //                </div>
                //            </div>`
                //});
                for (let i = 0; i < result.comment.length; i++){
                    tr += `<div class="card mb-4">
                                <div class="card-body">
                                    <p>${result.comment[i]}</p>

                                    <div class="d-flex justify-content-between">
                                        <div class="d-flex flex-row align-items-center">
                                            <img src="https://mdbcdn.b-cdn.net/img/Photos/Avatars/img%20(4).webp" alt="avatar" width="25"
                                             height="25" />
                                            <p class="small mb-0 ms-2">${result.fullName[i]}</p>
                                        </div>
                                        <div class="d-flex flex-row align-items-center">

                                            <p class="small text-muted mb-0">${result.rate[i]}</p>
                                        </div>
                                    </div>
                                </div>
                            </div>`
                }
                console.log(tr);
                $("#tablebody").html(tr);
            },
            error: (error) => {
                console.log(error);
            }
        });
    }
});