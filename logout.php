<?php
session_start();
include_once ('config.php');
if(isset($_SESSION['username']))
{
    session_destroy();
    echo "<script>alert('Bạn đã đăng xuất');</script>" ;
    echo "<script>window.location.href ='index.php'</script>";

}
else
{
    echo "<script>alert('Bạn chưa đăng nhập');</script>" ;
    echo "<script>window.location.href ='index.php'</script>";


}
?>