hideDoneItems = () => {
    $(".list-group-item-done").hide();
    $(".button-group-hide-done-button").hide();
    $(".button-group-show-done-button").show();
}

showDoneItems = () => {
    $(".list-group-item-done").show();
    $(".button-group-hide-done-button").show();
    $(".button-group-show-done-button").hide();
}