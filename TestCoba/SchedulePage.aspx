<%@ Page Title="" Language="C#" MasterPageFile="~/Visitor.Master" AutoEventWireup="true" CodeBehind="SchedulePage.aspx.cs" Inherits="TestCoba.SchedulePage" %>

<%@ Register Assembly="DayPilot" Namespace="DayPilot.Web.Ui" TagPrefix="DayPilot" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
        <div class="app-page-title">
            <div class="page-title-wrapper">
                <div class="page-title-heading">
                    <div class="page-title-icon">
                        <i class="pe-7s-car icon-gradient bg-mean-fruit">
                        </i>
                    </div>
                    <div>Schedule Jatuh Tempo
                        <div class="page-title-subheading">Halaman Untuk Melihat List Tanggal Jatuh Tempo.
                        </div>
                    </div>
                </div>
                <div class="page-title-actions">
                    <!--<button type="button" data-toggle="tooltip" title="Example Tooltip" data-placement="bottom" class="btn-shadow mr-3 btn btn-dark">
                        <i class="fa fa-star"></i>
                    </button>-->
                </div>    
            </div>
        </div>
        <div class="tab-content">
            <div class="tab-pane tabs-animation fade show active" id="tab-content-0" role="tabpanel">
                <div class="main-card mb-3 card">
                    <div class="card-body">
                        <!--<div id='calendar'></div>-->
                        <DayPilot:DayPilotCalendar ID="DayPilotCalendar1" runat="server" DataEndField="EventEnd" DataStartField="EventStart" DataTextField="Name" DataValueField="Id" TimeFormat="Clock24Hours" ViewType="Week" />
                    </div>
                </div>
            </div>
        </div>
</asp:Content>
