﻿using DamageMeter.Processing;
using Lang;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
using Tera.Game.Messages;
using static Tera.Game.Messages.S_CHAT;

namespace DamageMeter
{
    public class Chat
    {
        private static Chat _instance;

        private readonly LinkedList<ChatMessage> _chat = new LinkedList<ChatMessage>();
        private readonly int _maxMessage = 200;

        public enum ChatType
        {
            Whisper = 0,
            Normal = 1,
            PrivateChannel = 2
        }


        private Chat()
        {
        }

        public static Chat Instance => _instance ?? (_instance = new Chat());

        public void Add(S_CHAT message)
        {
            Add(message.Username, message.Text, ChatType.Normal, message.Channel);
        }

        public void Add(S_PRIVATE_CHAT message)
        {
            Add(message.AuthorName, message.Text, ChatType.PrivateChannel);
        }

        public void Add(S_WHISPER message)
        {
            Add(message.Sender, message.Text, ChatType.Whisper);
        }

        private void Add(string sender, string message, ChatType chatType, ChannelEnum? channel = null)
        {
            var time = DateTime.Now.ToString("HH:mm:ss");
            var rgx = new Regex("<[^>]+>");
            message = rgx.Replace(message, "");
            message = WebUtility.HtmlDecode(message);
            if (string.IsNullOrWhiteSpace(message)) { return; }

            if (_chat.Count == _maxMessage)
            {
                _chat.RemoveFirst();
            }


            
            if(chatType == ChatType.Whisper 
                && NetworkController.Instance.EntityTracker.MeterUser.Name != sender 
                && !TeraWindow.IsTeraActive())
            {
                NetworkController.Instance.FlashMessage = NotifyProcessor.DefaultNotifyAction(LP.Whisper +": "+sender, message);
            }

            if (chatType != ChatType.Whisper &&
                NetworkController.Instance.EntityTracker.MeterUser.Name != sender &&
                !TeraWindow.IsTeraActive() &&
                message.Contains("@"+ NetworkController.Instance.EntityTracker.MeterUser.Name))
            {
                NetworkController.Instance.FlashMessage = NotifyProcessor.DefaultNotifyAction( LP.Chat +": " + sender, message);
            }

            if ((chatType == ChatType.PrivateChannel ||
                (chatType == ChatType.Normal &&
                (channel == ChannelEnum.Group || channel == ChannelEnum.Guild || channel == ChannelEnum.Raid)))
                && !TeraWindow.IsTeraActive()
                && message.Contains("@@"))
            {
                NetworkController.Instance.FlashMessage = NotifyProcessor.DefaultNotifyAction("Wake up, "+ NetworkController.Instance.EntityTracker.MeterUser.Name, "Wake up, "+ NetworkController.Instance.EntityTracker.MeterUser.Name);
            }

            var chatMessage = new ChatMessage(sender, message, chatType, channel, time);
            _chat.AddLast(chatMessage);
        }

        public List<ChatMessage> Get()
        {
            return _chat.ToList();
        }
    }
}